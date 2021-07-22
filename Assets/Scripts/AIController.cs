using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    BoardController boardController;
    public delegate void ScoreHandler(int score, bool isPlayer);
    public static event ScoreHandler Score;
    System.Random random = new System.Random();

    int emptyLineCount = 0;

    GameObject bestChoice1;
    GameObject bestChoice2;

    private void Start()
    {
        boardController = GetComponent<BoardController>();
    }

    public void CheckBoxFilled(bool isHorizontal, GameObject obj)
    {
        GetComponent<BoxCollider2D>().enabled = false;
        RaycastHit2D hit;
        float rayLength = 100;
        boardController.adjacentLines1.Clear();
        boardController.adjacentLines2.Clear();
        if (isHorizontal)
        {
            hit = Physics2D.Raycast(obj.transform.position, -obj.transform.right, rayLength);
            {
                GameObject leftDot = hit.collider?.gameObject;
                if (leftDot != null)
                {
                    hit = Physics2D.Raycast(leftDot.transform.position, leftDot.transform.up, rayLength);
                    {
                        GameObject leftTopLine = hit.collider?.gameObject;
                        boardController.adjacentLines1.Add(leftTopLine);
                        if (leftTopLine != null)
                        {
                            hit = Physics2D.Raycast(leftTopLine.transform.position, leftTopLine.transform.up, rayLength);
                            {
                                GameObject leftTopDot = hit.collider?.gameObject;
                                if (leftTopDot != null)
                                {
                                    hit = Physics2D.Raycast(leftTopDot.transform.position, leftTopDot.transform.right, rayLength);
                                    {
                                        GameObject topLine = hit.collider?.gameObject;
                                        boardController.adjacentLines1.Add(topLine);
                                        if (topLine != null)
                                        {
                                            hit = Physics2D.Raycast(topLine.transform.position, topLine.transform.right, rayLength);
                                            {
                                                GameObject topRightDot = hit.collider?.gameObject;
                                                if (topRightDot != null)
                                                {
                                                    hit = Physics2D.Raycast(topRightDot.transform.position, -topRightDot.transform.up, rayLength);
                                                    {
                                                        GameObject rightTopLine = hit.collider?.gameObject;
                                                        boardController.adjacentLines1.Add(rightTopLine);
                                                    } 
                                                }
                                            } 
                                        }
                                    } 
                                }
                            } 
                        }
                    }
                    hit = Physics2D.Raycast(leftDot.transform.position, -leftDot.transform.up, rayLength);
                    {
                        GameObject leftBottomLine = hit.collider?.gameObject;
                        boardController.adjacentLines2.Add(leftBottomLine);
                        if (leftBottomLine != null)
                        {
                            hit = Physics2D.Raycast(leftBottomLine.transform.position, -leftBottomLine.transform.up, rayLength);
                            {
                                GameObject leftBottomDot = hit.collider?.gameObject;
                                if (leftBottomDot != null)
                                {
                                    hit = Physics2D.Raycast(leftBottomDot.transform.position, leftBottomDot.transform.right, rayLength);
                                    {
                                        GameObject bottomLine = hit.collider?.gameObject;
                                        boardController.adjacentLines2.Add(bottomLine);
                                        if (bottomLine != null)
                                        {
                                            hit = Physics2D.Raycast(bottomLine.transform.position, bottomLine.transform.right, rayLength);
                                            {
                                                GameObject bottomRightDot = hit.collider?.gameObject;
                                                if (bottomRightDot != null)
                                                {
                                                    hit = Physics2D.Raycast(bottomRightDot.transform.position, bottomRightDot.transform.up, rayLength);
                                                    {
                                                        GameObject rightBottomLine = hit.collider?.gameObject;
                                                        boardController.adjacentLines2.Add(rightBottomLine);
                                                    } 
                                                }
                                            } 
                                        }
                                    } 
                                }
                            } 
                        }
                    }
                }
            }
        }
        else
        {
            hit = Physics2D.Raycast(obj.transform.position, Vector2.up);
            {
                GameObject topDot = hit.collider?.gameObject;
                if (topDot != null)
                {
                    hit = Physics2D.Raycast(topDot.transform.position, -topDot.transform.right, rayLength);
                    {
                        GameObject leftTopLine = hit.collider?.gameObject;
                        boardController.adjacentLines1.Add(leftTopLine);
                        if (leftTopLine)
                        {
                            hit = Physics2D.Raycast(leftTopLine.transform.position, -leftTopLine.transform.right, rayLength);
                            {
                                GameObject leftTopDot = hit.collider?.gameObject;
                                if (leftTopDot != null)
                                {
                                    hit = Physics2D.Raycast(leftTopDot.transform.position, -leftTopDot.transform.up, rayLength);
                                    {
                                        GameObject leftLine = hit.collider?.gameObject;
                                        boardController.adjacentLines1.Add(leftLine);
                                        if (leftLine != null)
                                        {
                                            hit = Physics2D.Raycast(leftLine.transform.position, -leftLine.transform.up, rayLength);
                                            {
                                                GameObject bottomLeftDot = hit.collider?.gameObject;
                                                if (bottomLeftDot != null)
                                                {
                                                    hit = Physics2D.Raycast(bottomLeftDot.transform.position, bottomLeftDot.transform.right, rayLength);
                                                    {
                                                        GameObject leftBottomLine = hit.collider?.gameObject;
                                                        boardController.adjacentLines1.Add(leftBottomLine);
                                                    } 
                                                }
                                            } 
                                        }
                                    } 
                                }
                            }  
                        }
                    }
                }
                hit = Physics2D.Raycast(topDot.transform.position, topDot.transform.right, rayLength);
                {
                    GameObject rightTopLine = hit.collider?.gameObject;
                    boardController.adjacentLines2.Add(rightTopLine);
                    if (rightTopLine != null)
                    {
                        hit = Physics2D.Raycast(rightTopLine.transform.position, rightTopLine.transform.right, rayLength);
                        {
                            GameObject rightTopDot = hit.collider?.gameObject;
                            if (rightTopDot != null)
                            {
                                hit = Physics2D.Raycast(rightTopDot.transform.position, -rightTopDot.transform.up, rayLength);
                                {
                                    GameObject rightLine = hit.collider?.gameObject;
                                    boardController.adjacentLines2.Add(rightLine);
                                    if (rightLine != null)
                                    {
                                        hit = Physics2D.Raycast(rightLine.transform.position, -rightLine.transform.up, rayLength);
                                        {
                                            GameObject bottomrightDot = hit.collider?.gameObject;
                                            if (bottomrightDot != null)
                                            {
                                                hit = Physics2D.Raycast(bottomrightDot.transform.position, -bottomrightDot.transform.right, rayLength);
                                                {
                                                    GameObject rightBottomLine = hit.collider?.gameObject;
                                                    boardController.adjacentLines2.Add(rightBottomLine);
                                                } 
                                            }
                                        } 
                                    }
                                } 
                            }
                        } 
                    }
                }
            }
        }
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public bool CheckCompleteCellExists()
    {
        int scoredValue = 0;
        emptyLineCount = 0;
        bestChoice1 = null;
        bestChoice2 = null;

        foreach (GameObject obj in boardController.adjacentLines1)
        {
            if (obj == null)
            {
                //If obj is null, then there is no box.
                emptyLineCount = 3;
            }
            if (obj?.GetComponent<Line>().isLineDrawn == false)
            {
                if (emptyLineCount == 0 && bestChoice1 == null)
                {
                    bestChoice1 = obj;
                }
                else if (emptyLineCount > 0)
                {
                    bestChoice1 = null;
                }
                emptyLineCount++;
            }

        }

        if (emptyLineCount == 0)
        {
            scoredValue++;
        }

        emptyLineCount = 0;
        foreach (GameObject obj in boardController.adjacentLines2)
        {
            if (obj == null)
            {
                //If obj is null, then there is no box.
                emptyLineCount = 3;
            }
            if (obj?.GetComponent<Line>().isLineDrawn == false)
            {
                if (emptyLineCount == 0 && bestChoice2 == null)
                {
                    bestChoice2 = obj;
                }
                else if (emptyLineCount > 0)
                {
                    bestChoice2 = null;
                }
                emptyLineCount++;
            }
        }
        if (emptyLineCount == 0)
        {
            scoredValue++;
        }

        if (scoredValue > 0)
        {
            Score(scoredValue, BoardController.isPlayerTurn);
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameObject FindBestCell()
    {
        if (bestChoice1 != null)
        {
            return bestChoice1;
        }
        else if (bestChoice1 != null)
        {
            return bestChoice2;
        }
        else
        {
            int num = random.Next(boardController.totalLineCount - 1);
            if (boardController.lines[num].GetComponent<Line>().isLineDrawn == true)
            {
                return FindBestCell();
            }
            else
            {
                return boardController.lines[num];
            }
        }
    }
}
