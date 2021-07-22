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

    private int dotGap = 10;
    private int offset;
    private int rowCount = 3;
    private int columnCount = 3;

    private bool isGameOver = false;

    private Vector3[,] dots;

    public int totalLineCount = 0;
    private int totalCompletedLines = 0;
    public GameObject[] lines;

    public delegate void GameEvent();
    public static event GameEvent GameEnded;
    public static event GameEvent TurnChanged;

    private BoardSizeData boardSizeData;

    public static bool isPlayerTurn = true;

    private void Start()
    {
        boardSizeData = GameObject.FindGameObjectWithTag("Player").GetComponent<BoardSizeData>();
        rowCount = boardSizeData.rowValue;
        columnCount = boardSizeData.columnValue;
        rowCount++;
        columnCount++;
        cam = Camera.main;
        SetCameraPos();
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
            for (int index = 0; (index < columnCount && lineIndex < totalLineCount); index++)
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
        
        transform.GetComponent<BoxCollider2D>().offset = new Vector2(((float)rowCount * dotGap / 2 - 0.5f), ((float)columnCount * dotGap / 2 - 0.5f));
        transform.GetComponent<BoxCollider2D>().size = new Vector2(rowCount * dotGap + (dotGap / 2), columnCount * dotGap + (dotGap / 2));
    }

    private void SetCameraPos()
    {
        cam.transform.position = new Vector3(((float)columnCount * dotGap / 2 - 5), ((float)rowCount * dotGap / 2), -10);
        cam.orthographicSize = (rowCount > columnCount) ? dotGap * rowCount : dotGap * columnCount;
    }

    private void OnMouseDown()
    {
        if (isPlayerTurn && totalCompletedLines != totalLineCount)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PlayerAction(mousePos);
        }
    }

    private IEnumerator WaitAndPrint()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            
            if (!isPlayerTurn && totalCompletedLines != totalLineCount)
            {
                GameObject obj = aiController.FindBestCell();
                PlayerAction(obj.transform.position);
            }
            else if (totalCompletedLines == totalLineCount)
            {
                GameEnded();
                gameObject.SetActive(false);
                break;
            }
        }
    }

    private void PlayerAction(Vector3 mousePos)
    {
        bool isLineCheckComplete = false;
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
                            if (!aiController.CheckCompleteCellExists())
                            {
                                isPlayerTurn = !isPlayerTurn;
                                TurnChanged();
                            }
                            isLineCheckComplete = true;
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
                            if (!aiController.CheckCompleteCellExists())
                            {
                                isPlayerTurn = !isPlayerTurn;
                                TurnChanged();
                            }
                            isLineCheckComplete = true;
                            break;
                        }
                    }
                }
                if (isLineCheckComplete)
                { 
                    break;
                }
            }
            if (isLineCheckComplete)
            {
                break;
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
        totalCompletedLines++; 
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
