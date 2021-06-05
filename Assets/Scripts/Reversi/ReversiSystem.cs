using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReversiSystem : MonoBehaviour
{
    const int m_columns = 8;
    const int m_rows = 8;
    [SerializeField] ReversiCell m_ReversiCellPrefab = null;
    [SerializeField] GameObject m_parent = null;
    [SerializeField] Text m_turnText = null;
    [SerializeField] Text m_whiteCellNumText = null;
    [SerializeField] Text m_blackCellNumText = null;
    public static ReversiCell[,] reversiCells = new ReversiCell[m_columns, m_rows];
    public static TurnState m_turnState = TurnState.WhiteTurn;
    public static bool isChecked = true;
    public static List<ReversiCell> turnOverList = new List<ReversiCell>();
    public static List<ReversiCell> PlaceableList = new List<ReversiCell>();
    int whiteCellTotal = 0;
    int blackCellTotal = 0;

    public enum TurnState
    {
        WhiteTurn,
        BlackTurn,
        EndGame
    }

    void Start()
    {
        for (int i = 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                var cell = Instantiate(m_ReversiCellPrefab);
                cell.transform.SetParent(m_parent.transform);
                reversiCells[n, i] = cell;
                cell.transform.position = new Vector3(n, 0, i);
                cell.m_cell_X = n;
                cell.m_cell_Y = i;
                if (n == 4 && i == 3 || n == 3 && i == 4) cell.ReversiCellState = ReversiCell.ReversiCellStates.White;
                if (n == 3 && i == 3 || n == 4 && i == 4) cell.ReversiCellState = ReversiCell.ReversiCellStates.Black;
            }
        }
    }

    void Update()
    {
        switch (m_turnState)
        {
            case TurnState.WhiteTurn:
                if (isChecked)
                {
                    WhiteThinking();
                    PieceNumCheck();
                    isChecked = false;
                    Debug.Log("白のチェック完了");
                    m_turnText.text = "白のターン";
                    m_whiteCellNumText.text = "白の数：" + whiteCellTotal.ToString() + "個";
                    m_blackCellNumText.text = "黒の数：" + blackCellTotal.ToString() + "個";
                }
                break;
            case TurnState.BlackTurn:
                if (isChecked)
                {
                    BlackThinking();
                    PieceNumCheck();
                    isChecked = false;
                    Debug.Log("黒のチェック完了");
                    m_turnText.text = "黒のターン";
                    m_whiteCellNumText.text = "白の数：" + whiteCellTotal.ToString() + "個";
                    m_blackCellNumText.text = "黒の数：" + blackCellTotal.ToString() + "個";
                }
                break;
            case TurnState.EndGame:
                break;
        }
    }

    public void WhiteThinking()
    {
        for (int i = 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                if (reversiCells[n, i].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    continue;
                }
                else if (reversiCells[n, i].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    turnOverList.Clear();
                    CheckCells(n, i, reversiCells[n, i].ReversiCellState);
                }
            }
        }
    }

    public void BlackThinking()
    {
        for (int i = 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                if (reversiCells[n, i].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    continue;
                }
                else if (reversiCells[n, i].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    turnOverList.Clear();
                    CheckCells(n, i, reversiCells[n, i].ReversiCellState);
                }
            }
        }
    }

    public void CheckCells(int x, int y, ReversiCell.ReversiCellStates states)
    {
        int bottom = y - 1;
        int top = y + 1;
        int right = x + 1;
        int left = x - 1;

        if (states == ReversiCell.ReversiCellStates.White)
        {
            if (bottom >= 0)
            {
                if (left >= 0)
                {
                    if (reversiCells[left, bottom].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                    {
                        //turnOverList.Add(reversiCells[left, bottom]);
                        LeftBottomCheck(left, bottom, states);
                    }
                }

                
                if (reversiCells[x, bottom].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    //turnOverList.Add(reversiCells[x, bottom]);
                    BottomCheck(x, bottom, states);
                }

                if (right < m_columns)
                {
                     if (reversiCells[right, bottom].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                    {
                        //turnOverList.Add(reversiCells[right, bottom]);
                        RightBottomCheck(right, bottom, states);
                    }
                }
            }

            if (left >= 0)
            {
                if (reversiCells[left, y].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    //turnOverList.Add(reversiCells[left, y]);
                    LeftCheck(left, y, states);
                }
            }
            if (right < m_columns)
            {
               if (reversiCells[right, y].ReversiCellState == ReversiCell.ReversiCellStates.Black)
               {
                    //turnOverList.Add(reversiCells[right, y]);
                    RightCheck(right, y, states);
               }
            }

            if (top < m_rows)
            {
                if (left >= 0)
                {
                    if (reversiCells[left, top].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                    {
                        //turnOverList.Add(reversiCells[left, top]);
                        LeftTopCheck(left, top, states);
                    }
                }

                if (reversiCells[x, top].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    //turnOverList.Add(reversiCells[x, top]);
                    TopCheck(x, top, states);
                }

                if (right < m_columns)
                {
                    if (reversiCells[right, top].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                    {
                        //turnOverList.Add(reversiCells[right, top]);
                        RightTopCheck(right, top, states);
                    }
                }
            }
        }
        else if (reversiCells[x, y].ReversiCellState == ReversiCell.ReversiCellStates.Black)
        {
            if (bottom >= 0)
            {
                if (left >= 0)
                {
                    if (reversiCells[left, bottom].ReversiCellState == ReversiCell.ReversiCellStates.White)
                    {
                        //turnOverList.Add(reversiCells[left, bottom]);
                        LeftBottomCheck(left, bottom, states);
                    }
                }

                if (reversiCells[x, bottom].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    //turnOverList.Add(reversiCells[x, bottom]);
                    BottomCheck(x, bottom, states);
                }

                if (right < m_columns)
                {
                    if (reversiCells[right, bottom].ReversiCellState == ReversiCell.ReversiCellStates.White)
                    {
                        //turnOverList.Add(reversiCells[right, bottom]);
                        RightBottomCheck(right, bottom, states);
                    }
                }
            }

            if (left >= 0)
            {
                if (reversiCells[left, y].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    //turnOverList.Add(reversiCells[left, y]);
                    LeftCheck(left, y, states);
                }
            }
            if (right < m_columns)
            {
                if (reversiCells[right, y].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    //turnOverList.Add(reversiCells[right, y]);
                    RightCheck(right, y, states);
                }
            }

            if (top < m_rows)
            {
                if (left >= 0)
                {
                    if (reversiCells[left, top].ReversiCellState == ReversiCell.ReversiCellStates.White)
                    {
                        //turnOverList.Add(reversiCells[left, top]);
                        LeftTopCheck(left, top, states);
                    }
                }

                if (reversiCells[x, top].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    //turnOverList.Add(reversiCells[x, top]);
                    TopCheck(x, top, states);
                }

                if (right < m_columns)
                {
                    if (reversiCells[right, top].ReversiCellState == ReversiCell.ReversiCellStates.White)
                    {
                        //turnOverList.Add(reversiCells[right, top]);
                        RightTopCheck(right, top, states);
                    }
                }
            }
        }
    }

    public void LeftBottomCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (x > 0 && y > 0)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x - 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    //turnOverList.Add(reversiCells[x - 1, y - 1]);
                    LeftBottomCheck(x - 1, y - 1, states);
                }
                else if (reversiCells[x - 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    reversiCells[x - 1, y - 1].isWhitePlaceable = true;
                    PlaceableList.Add(reversiCells[x - 1, y - 1]);
                }
                else
                {
                    return;
                }
            }
            else if (states == ReversiCell.ReversiCellStates.Black)
            {
                if (reversiCells[x - 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    turnOverList.Add(reversiCells[x - 1, y - 1]);
                    LeftBottomCheck(x - 1, y - 1, states);
                }
                else if (reversiCells[x - 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    reversiCells[x - 1, y - 1].isBlackPlaceable = true;
                    PlaceableList.Add(reversiCells[x - 1, y - 1]);
                }
                else
                {
                    return;
                }
            }
        }     
    }

    public void BottomCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (y > 0)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    turnOverList.Add(reversiCells[x, y - 1]);
                    LeftBottomCheck(x, y - 1, states);
                }
                else if (reversiCells[x, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    reversiCells[x, y - 1].isWhitePlaceable = true;
                    PlaceableList.Add(reversiCells[x, y - 1]);
                }
                else
                {
                    return;
                }
            }
            else if (states == ReversiCell.ReversiCellStates.Black)
            {
                if (reversiCells[x, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    turnOverList.Add(reversiCells[x, y - 1]);
                    LeftBottomCheck(x, y - 1, states);
                }
                else if (reversiCells[x, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    reversiCells[x, y - 1].isBlackPlaceable = true;
                    PlaceableList.Add(reversiCells[x, y - 1]);
                }
                else
                {
                    return;
                }
            }
        }   
    }

    public void RightBottomCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (x < m_columns && y > 0)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x + 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    turnOverList.Add(reversiCells[x + 1, y - 1]);
                    LeftBottomCheck(x + 1, y - 1, states);
                }
                else if (reversiCells[x + 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    reversiCells[x + 1, y - 1].isWhitePlaceable = true;
                    PlaceableList.Add(reversiCells[x + 1, y - 1]);
                }
                else
                {
                    return;
                }
            }
            else if (states == ReversiCell.ReversiCellStates.Black)
            {
                if (reversiCells[x + 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    turnOverList.Add(reversiCells[x + 1, y - 1]);
                    LeftBottomCheck(x + 1, y - 1, states);
                }
                else if (reversiCells[x + 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    reversiCells[x + 1, y - 1].isBlackPlaceable = true;
                    PlaceableList.Add(reversiCells[x + 1, y - 1]);
                }
                else
                {
                    return;
                }
            }
        }       
    }

    public void RightCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (x < m_columns - 1)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x + 1, y].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    turnOverList.Add(reversiCells[x + 1, y]);
                    LeftBottomCheck(x + 1, y, states);
                }
                else if (reversiCells[x + 1, y].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    reversiCells[x + 1, y].isWhitePlaceable = true;
                    PlaceableList.Add(reversiCells[x + 1, y]);
                }
                else
                {
                    return;
                }
            }
            else if (states == ReversiCell.ReversiCellStates.Black)
            {
                if (reversiCells[x + 1, y].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    turnOverList.Add(reversiCells[x + 1, y]);
                    LeftBottomCheck(x + 1, y, states);
                }
                else if (reversiCells[x + 1, y].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    reversiCells[x + 1, y].isBlackPlaceable = true;
                    PlaceableList.Add(reversiCells[x + 1, y]);
                }
                else
                {
                    return;
                }
            }
        }       
    }

    public void RightTopCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (states == ReversiCell.ReversiCellStates.White)
        {
            if (reversiCells[x + 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
            {
                turnOverList.Add(reversiCells[x + 1, y + 1]);
                LeftBottomCheck(x + 1, y + 1, states);
            }
            else if (reversiCells[x + 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
            {
                reversiCells[x + 1, y + 1].isWhitePlaceable = true;
                PlaceableList.Add(reversiCells[x + 1, y + 1]);
            }
            else
            {
                return;
            }
        }
        else if (states == ReversiCell.ReversiCellStates.Black)
        {
            if (reversiCells[x + 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.White)
            {
                turnOverList.Add(reversiCells[x + 1, y + 1]);
                LeftBottomCheck(x + 1, y + 1, states);
            }
            else if (reversiCells[x + 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
            {
                reversiCells[x + 1, y + 1].isBlackPlaceable = true;
                PlaceableList.Add(reversiCells[x + 1, y + 1]);
            }
            else
            {
                return;
            }
        }
    }

    public void TopCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (y < m_rows - 1)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    turnOverList.Add(reversiCells[x, y + 1]);
                    LeftBottomCheck(x, y + 1, states);
                }
                else if (reversiCells[x, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    reversiCells[x, y + 1].isWhitePlaceable = true;
                    PlaceableList.Add(reversiCells[x, y + 1]);
                }
                else
                {
                    return;
                }
            }
            else if (states == ReversiCell.ReversiCellStates.Black)
            {
                if (reversiCells[x, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    turnOverList.Add(reversiCells[x, y + 1]);
                    LeftBottomCheck(x, y + 1, states);
                }
                else if (reversiCells[x, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    reversiCells[x, y + 1].isBlackPlaceable = true;
                    PlaceableList.Add(reversiCells[x, y + 1]);
                }
                else
                {
                    return;
                }
            }
        }     
    }

    public void LeftTopCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (states == ReversiCell.ReversiCellStates.White)
        {
            if (reversiCells[x - 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
            {
                turnOverList.Add(reversiCells[x - 1, y + 1]);
                LeftBottomCheck(x - 1, y + 1, states);
            }
            else if (reversiCells[x - 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
            {
                reversiCells[x - 1, y + 1].isWhitePlaceable = true;
                PlaceableList.Add(reversiCells[x - 1, y + 1]);
            }
            else
            {
                return;
            }
        }
        else if (states == ReversiCell.ReversiCellStates.Black)
        {
            if (reversiCells[x - 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.White)
            {
                turnOverList.Add(reversiCells[x - 1, y + 1]);
                LeftBottomCheck(x - 1, y + 1, states);
            }
            else if (reversiCells[x - 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
            {
                reversiCells[x - 1, y + 1].isBlackPlaceable = true;
                PlaceableList.Add(reversiCells[x - 1, y + 1]);
            }
            else
            {
                return;
            }
        }
    }

    public void LeftCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (states == ReversiCell.ReversiCellStates.White)
        {
            if (x > 0)
            {
                if (reversiCells[x - 1, y].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    turnOverList.Add(reversiCells[x - 1, y]);
                    LeftBottomCheck(x - 1, y, states);
                }
                else if (reversiCells[x - 1, y].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    reversiCells[x - 1, y].isWhitePlaceable = true;
                    PlaceableList.Add(reversiCells[x - 1, y]);
                }
                else
                {
                    return;
                }
            }         
        }
        else if (states == ReversiCell.ReversiCellStates.Black)
        {
            if (x > 0)
            {
                if (reversiCells[x - 1, y].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    turnOverList.Add(reversiCells[x - 1, y]);
                    LeftBottomCheck(x - 1, y, states);
                }
                else if (reversiCells[x - 1, y].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    reversiCells[x - 1, y].isBlackPlaceable = true;
                    PlaceableList.Add(reversiCells[x - 1, y]);
                }
                else
                {
                    return;
                }
            }      
        }
    }

    public void PieceNumCheck()
    {
        whiteCellTotal = 0;
        blackCellTotal = 0;

        for (int i = 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                if (reversiCells[n, i].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    whiteCellTotal++;
                }
                else if (reversiCells[n, i].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    blackCellTotal++;
                }
                else
                {
                    continue;
                }
            }
        }
    }
}
