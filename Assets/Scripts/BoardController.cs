using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField]
    private GameObject dot;
    [SerializeField]
    private GameObject line;

    private AIController aiController;

    public List<GameObject> adjacentLines1;
    public List<GameObject> adjacentLines2;

    private Camera cam;

    private int rowCount = 7;
    private int columnCount = 7;
    private int dotGap = 10;
    private int offset;

    private bool isGameOver = false;

    private Vector3[,] dots;

    public int totalLineCount = 0;
    public bool isPlayerTurn = true;
    public GameObject[] lines;


    private void Start()
    {
        cam = Camera.main;
        cam.orthographicSize = dotGap * columnCount; 
        dots = new Vector3[rowCount, columnCount];
        totalLineCount = (rowCount * (columnCount - 1)) + ((rowCount - 1) * columnCount);
        lines = new GameObject[totalLineCount];
        adjacentLines1 = new List<GameObject>(3);
        adjacentLines2 = new List<GameObject>(3);
        offset = dotGap / 10;
        aiController = GetComponent<AIController>();
        GenerateGrid();
        StartCoroutine(WaitAndPrint());
    }

    void GenerateGrid()
    {
        int lineIndex = 0;
        Vector2 pos = new Vector2();
        Vector3 colliderSize = new Vector3(0, 0, 0.2f);
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                GameObject spawnedDot = Instantiate(dot, new Vector2(j * dotGap, i * dotGap), Quaternion.identity);
                spawnedDot.name = "Dot " + j + " " + i;
                spawnedDot.transform.parent = transform;
                dots[i,j] = spawnedDot.transform.position;

                if (j % (columnCount - 1) != 0 || j == 0)
                {
                    lines[lineIndex] = Instantiate(line);
                    lines[lineIndex].transform.parent = transform;
                    pos.x = j * dotGap + (dotGap / 2);
                    pos.y = i * dotGap;
                    lines[lineIndex].transform.position = pos;
                    lines[lineIndex].name = "Line H " + j + " " + i;
                    lines[lineIndex].AddComponent<Line>();
                    colliderSize.x = dotGap - offset;
                    colliderSize.y = 1;
                    lines[lineIndex].GetComponent<BoxCollider2D>().size = colliderSize;
                    lineIndex++;
                }
            }
            for (int index = 0; (index < rowCount && lineIndex < totalLineCount); index++)
            {
                lines[lineIndex] = Instantiate(line);
                lines[lineIndex].transform.parent = transform;
                pos.x = index * dotGap;
                pos.y = i * dotGap + (dotGap / 2);
                lines[lineIndex].transform.position = pos;
                lines[lineIndex].name = "Line V " + i + " " + index;
                colliderSize.x = 1;
                colliderSize.y = dotGap - offset;
                lines[lineIndex].GetComponent<BoxCollider2D>().size = colliderSize;
                lines[lineIndex].AddComponent<Line>();
                lineIndex++;
            }
        }
        cam.transform.position = new Vector3(((float)rowCount * dotGap / 2 - 5), ((float)columnCount * dotGap / 2), -10);
        transform.GetComponent<BoxCollider2D>().offset = new Vector2(((float)rowCount * dotGap / 2 - 0.5f), ((float)columnCount * dotGap / 2 - 0.5f));
        transform.GetComponent<BoxCollider2D>().size = new Vector2(rowCount * dotGap, columnCount * dotGap);
    }

    private void OnMouseDown()
    {
        if (isPlayerTurn)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PlayerAction(mousePos);
        }
    }

    private IEnumerator WaitAndPrint()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (!isPlayerTurn)
            {
                GameObject obj = aiController.FindBestCell();
                PlayerAction(obj.transform.position);
            }
        }
    }

    private void PlayerAction(Vector3 mousePos)
    {
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                //Horizontal lines
                if (mousePos.x > dots[i, j].x && (j == columnCount - 1 || mousePos.x < dots[i, j + 1].x) && mousePos.y > dots[i, j].y - offset && mousePos.y < dots[i, j].y + offset)
                {
                    foreach (GameObject go in lines)
                    {
                        if (go.name == "Line H " + j + " " + i && !go.GetComponent<Line>().isLineDrawn)
                        {
                            DrawLine(go, i, j, true);
                            aiController.CheckCompleteCellExists();
                            isPlayerTurn = !isPlayerTurn;
                            break;
                        }
                    }
                }
                //Vertical lines
                else if (mousePos.y > dots[i, j].y && (i == rowCount - 1 || mousePos.y < dots[i + 1, j].y) && mousePos.x > dots[i, j].x - offset && mousePos.x < dots[i, j].x + offset)
                {
                    foreach (GameObject go in lines)
                    {
                        if (go.name == "Line V " + i + " " + j && !go.GetComponent<Line>().isLineDrawn)
                        {
                            DrawLine(go, i, j, false);
                            aiController.CheckCompleteCellExists();
                            isPlayerTurn = !isPlayerTurn;
                            break;
                        }
                    }
                }
            }
        }
    }

    private void DrawLine(GameObject obj, int i, int j, bool isHorizontal)
    {
        LineRenderer line = obj.transform.GetComponent<LineRenderer>();
        line.SetPosition(0, dots[i, j]);
        if(isHorizontal)
            line.SetPosition(1, dots[i, j + 1]);
        else
            line.SetPosition(1, dots[i + 1, j]);
        ChangeTurn(line);
        aiController.CheckBoxFilled(isHorizontal, obj);
    }


    private void ChangeTurn(LineRenderer line)
    {
        if (isPlayerTurn)
        {
            line.startColor = Color.blue;
            line.endColor = Color.blue;
        }
        else
        {
            line.startColor = Color.green;
            line.endColor = Color.green;
        }
        line.GetComponent<Line>().isLineDrawn = true;
    }
}
