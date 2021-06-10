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
    [SerializeField] Text m_winnerText = null;
    [SerializeField] GameObject m_FinishedPanel = null;
    public static ReversiCell[,] reversiCells = new ReversiCell[m_columns, m_rows];
    public static TurnState m_turnState = TurnState.WhiteTurn;
    public static bool isChecked = true;
    public static List<ReversiCell> LBturnOverList = new List<ReversiCell>();
    public static List<ReversiCell> LturnOverList = new List<ReversiCell>();
    public static List<ReversiCell> LTturnOverList = new List<ReversiCell>();
    public static List<ReversiCell> RBturnOverList = new List<ReversiCell>();
    public static List<ReversiCell> RturnOverList = new List<ReversiCell>();
    public static List<ReversiCell> RTturnOverList = new List<ReversiCell>();
    public static List<ReversiCell> BturnOverList = new List<ReversiCell>();
    public static List<ReversiCell> TturnOverList = new List<ReversiCell>();
    public static List<ReversiCell> PlaceableList = new List<ReversiCell>();
    int whiteCellTotal = 2;
    int blackCellTotal = 2;

    public enum TurnState
    {
        WhiteTurn,
        BlackTurn,
        EndGame
    }

    void Start()
    {
        int r = Random.Range(0, 2);
        if (r == 0)
        {
            m_turnState = TurnState.WhiteTurn;
        }
        else
        {
            m_turnState = TurnState.BlackTurn;
        }

        m_winnerText.enabled = false;
        m_FinishedPanel.SetActive(false);

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
        if (whiteCellTotal + blackCellTotal == m_columns * m_rows || whiteCellTotal == 0 || blackCellTotal == 0)
        {
            m_turnState = TurnState.EndGame;
        }

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
                    if (PlaceableList.Count == 0) { m_turnState = TurnState.BlackTurn; isChecked = true; }
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
                    if (PlaceableList.Count == 0) { m_turnState = TurnState.WhiteTurn; isChecked = true; }
                }
                break;
            case TurnState.EndGame:
                m_FinishedPanel.SetActive(true);
                m_winnerText.enabled = true;

                if (whiteCellTotal > blackCellTotal)
                {
                    m_winnerText.text = "<color=#FFFFFF>白</color><color=#F10000>の勝利！</color>";
                }
                else if (blackCellTotal > whiteCellTotal)
                {
                    m_winnerText.text = "<color=#4C4C4C>黒</color><color=#F10000>の勝利！</color>";
                }
                else
                {
                    m_winnerText.text = "引き分け";
                }
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
                        LeftBottomCheck(left, bottom, states);
                    }
                }

                
                if (reversiCells[x, bottom].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    BottomCheck(x, bottom, states);
                }

                if (right < m_columns)
                {
                     if (reversiCells[right, bottom].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                    {
                        RightBottomCheck(right, bottom, states);
                    }
                }
            }

            if (left >= 0)
            {
                if (reversiCells[left, y].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    LeftCheck(left, y, states);
                }
            }
            if (right < m_columns)
            {
               if (reversiCells[right, y].ReversiCellState == ReversiCell.ReversiCellStates.Black)
               {
                    RightCheck(right, y, states);
               }
            }

            if (top < m_rows)
            {
                if (left >= 0)
                {
                    if (reversiCells[left, top].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                    {
                        LeftTopCheck(left, top, states);
                    }
                }

                if (reversiCells[x, top].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    TopCheck(x, top, states);
                }

                if (right < m_columns)
                {
                    if (reversiCells[right, top].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                    {
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
                        LeftBottomCheck(left, bottom, states);
                    }
                }

                if (reversiCells[x, bottom].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    BottomCheck(x, bottom, states);
                }

                if (right < m_columns)
                {
                    if (reversiCells[right, bottom].ReversiCellState == ReversiCell.ReversiCellStates.White)
                    {
                        RightBottomCheck(right, bottom, states);
                    }
                }
            }

            if (left >= 0)
            {
                if (reversiCells[left, y].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    LeftCheck(left, y, states);
                }
            }
            if (right < m_columns)
            {
                if (reversiCells[right, y].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    RightCheck(right, y, states);
                }
            }

            if (top < m_rows)
            {
                if (left >= 0)
                {
                    if (reversiCells[left, top].ReversiCellState == ReversiCell.ReversiCellStates.White)
                    {
                        LeftTopCheck(left, top, states);
                    }
                }

                if (reversiCells[x, top].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    TopCheck(x, top, states);
                }

                if (right < m_columns)
                {
                    if (reversiCells[right, top].ReversiCellState == ReversiCell.ReversiCellStates.White)
                    {
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
                    BottomCheck(x, y - 1, states);
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
                    BottomCheck(x, y - 1, states);
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
        if (x < m_columns - 1 && y > 0)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x + 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    RightBottomCheck(x + 1, y - 1, states);
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
                    RightBottomCheck(x + 1, y - 1, states);
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
                    RightCheck(x + 1, y, states);
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
                    RightCheck(x + 1, y, states);
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
        if (x < m_columns - 1 && y < m_rows - 1)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x + 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    RightTopCheck(x + 1, y + 1, states);
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
                    RightTopCheck(x + 1, y + 1, states);
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
    }

    public void TopCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (y < m_rows - 1)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    TopCheck(x, y + 1, states);
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
                    TopCheck(x, y + 1, states);
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
        if (x > 0 && y < m_rows - 1)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x - 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    LeftTopCheck(x - 1, y + 1, states);
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
                    LeftTopCheck(x - 1, y + 1, states);
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
    }

    public void LeftCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (states == ReversiCell.ReversiCellStates.White)
        {
            if (x > 0)
            {
                if (reversiCells[x - 1, y].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    LeftCheck(x - 1, y, states);
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
                    LeftCheck(x - 1, y, states);
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

    public static void AddTurnOverCells(int x, int y, ReversiCell.ReversiCellStates states)
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
                        LBturnOverList.Clear();
                        LBturnOverList.Add(reversiCells[left, bottom]);
                        SLeftBottomCheck(left, bottom, states);
                    }
                }


                if (reversiCells[x, bottom].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    BturnOverList.Clear();
                    BturnOverList.Add(reversiCells[x, bottom]);
                    SBottomCheck(x, bottom, states);
                }

                if (right < m_columns)
                {
                    if (reversiCells[right, bottom].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                    {
                        RBturnOverList.Clear();
                        RBturnOverList.Add(reversiCells[right, bottom]);
                        SRightBottomCheck(right, bottom, states);
                    }
                }
            }

            if (left >= 0)
            {
                if (reversiCells[left, y].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    LturnOverList.Clear();
                    LturnOverList.Add(reversiCells[left, y]);
                    SLeftCheck(left, y, states);
                }
            }
            if (right < m_columns)
            {
                if (reversiCells[right, y].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    RturnOverList.Clear();
                    RturnOverList.Add(reversiCells[right, y]);
                    SRightCheck(right, y, states);
                }
            }

            if (top < m_rows)
            {
                if (left >= 0)
                {
                    if (reversiCells[left, top].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                    {
                        LTturnOverList.Clear();
                        LTturnOverList.Add(reversiCells[left, top]);
                        SLeftTopCheck(left, top, states);
                    }
                }

                if (reversiCells[x, top].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    TturnOverList.Clear();
                    TturnOverList.Add(reversiCells[x, top]);
                    STopCheck(x, top, states);
                }

                if (right < m_columns)
                {
                    if (reversiCells[right, top].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                    {
                        RTturnOverList.Clear();
                        RTturnOverList.Add(reversiCells[right, top]);
                        SRightTopCheck(right, top, states);
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
                        LBturnOverList.Clear();
                        LBturnOverList.Add(reversiCells[left, bottom]);
                        SLeftBottomCheck(left, bottom, states);
                    }
                }

                if (reversiCells[x, bottom].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    BturnOverList.Clear();
                    BturnOverList.Add(reversiCells[x, bottom]);
                    SBottomCheck(x, bottom, states);
                }

                if (right < m_columns)
                {
                    if (reversiCells[right, bottom].ReversiCellState == ReversiCell.ReversiCellStates.White)
                    {
                        RBturnOverList.Clear();
                        RBturnOverList.Add(reversiCells[right, bottom]);
                        SRightBottomCheck(right, bottom, states);
                    }
                }
            }

            if (left >= 0)
            {
                if (reversiCells[left, y].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    LturnOverList.Clear();
                    LturnOverList.Add(reversiCells[left, y]);
                    SLeftCheck(left, y, states);
                }
            }
            if (right < m_columns)
            {
                if (reversiCells[right, y].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    RturnOverList.Clear();
                    RturnOverList.Add(reversiCells[right, y]);
                    SRightCheck(right, y, states);
                }
            }

            if (top < m_rows)
            {
                if (left >= 0)
                {
                    if (reversiCells[left, top].ReversiCellState == ReversiCell.ReversiCellStates.White)
                    {
                        LTturnOverList.Clear();
                        LTturnOverList.Add(reversiCells[left, top]);
                        SLeftTopCheck(left, top, states);
                    }
                }

                if (reversiCells[x, top].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    TturnOverList.Clear();
                    TturnOverList.Add(reversiCells[x, top]);
                    STopCheck(x, top, states);
                }

                if (right < m_columns)
                {
                    if (reversiCells[right, top].ReversiCellState == ReversiCell.ReversiCellStates.White)
                    {
                        RTturnOverList.Clear();
                        RTturnOverList.Add(reversiCells[right, top]);
                        SRightTopCheck(right, top, states);
                    }
                }
            }
        }
    }
    public static void SLeftBottomCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (x > 0 && y > 0)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x - 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    LBturnOverList.Add(reversiCells[x - 1, y - 1]);
                    SLeftBottomCheck(x - 1, y - 1, states);
                }
                else if (reversiCells[x - 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    LBturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in LBturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.White;
                        cell.m_anim.Play("WhiteAnimation");
                    }
                    return;
                }
            }
            else if (states == ReversiCell.ReversiCellStates.Black)
            {
                if (reversiCells[x - 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    LBturnOverList.Add(reversiCells[x - 1, y - 1]);
                    SLeftBottomCheck(x - 1, y - 1, states);
                }
                else if (reversiCells[x - 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    LBturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in LBturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.Black;
                        cell.m_anim.Play("BlackAnimation");
                    }
                    return;
                }
            }
        }
    }

    public static void SBottomCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (y > 0)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    BturnOverList.Add(reversiCells[x, y - 1]);
                    SBottomCheck(x, y - 1, states);
                }
                else if (reversiCells[x, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    BturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in BturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.White;
                        cell.m_anim.Play("WhiteAnimation");
                    }
                    return;
                }
            }
            else if (states == ReversiCell.ReversiCellStates.Black)
            {
                if (reversiCells[x, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    BturnOverList.Add(reversiCells[x, y - 1]);
                    SBottomCheck(x, y - 1, states);
                }
                else if (reversiCells[x, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    BturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in BturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.Black;
                        cell.m_anim.Play("BlackAnimation");
                    }
                    return;
                }
            }
        }
    }

    public static void SRightBottomCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (x < m_columns - 1 && y > 0)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x + 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    RBturnOverList.Add(reversiCells[x + 1, y - 1]);
                    SRightBottomCheck(x + 1, y - 1, states);
                }
                else if (reversiCells[x + 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    RBturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in RBturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.White;
                        cell.m_anim.Play("WhiteAnimation");
                    }
                    return;
                }
            }
            else if (states == ReversiCell.ReversiCellStates.Black)
            {
                if (reversiCells[x + 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    RBturnOverList.Add(reversiCells[x + 1, y - 1]);
                    SRightBottomCheck(x + 1, y - 1, states);
                }
                else if (reversiCells[x + 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    RBturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in RBturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.Black;
                        cell.m_anim.Play("BlackAnimation");
                    }
                    return;
                }
            }
        }
    }

    public static void SRightCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (x < m_columns - 1)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x + 1, y].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    RturnOverList.Add(reversiCells[x + 1, y]);
                    SRightCheck(x + 1, y, states);
                }
                else if (reversiCells[x + 1, y].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    RturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in RturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.White;
                        cell.m_anim.Play("WhiteAnimation");
                    }
                    return;
                }
            }
            else if (states == ReversiCell.ReversiCellStates.Black)
            {
                if (reversiCells[x + 1, y].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    RturnOverList.Add(reversiCells[x + 1, y]);
                    SRightCheck(x + 1, y, states);
                }
                else if (reversiCells[x + 1, y].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    RturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in RturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.Black;
                        cell.m_anim.Play("BlackAnimation");
                    }
                    return;
                }
            }
        }
    }

    public static void SRightTopCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (x < m_columns - 1 && y < m_rows - 1)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x + 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    RTturnOverList.Add(reversiCells[x + 1, y + 1]);
                    SRightTopCheck(x + 1, y + 1, states);
                }
                else if (reversiCells[x + 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    RTturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in RTturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.White;
                        cell.m_anim.Play("WhiteAnimation");
                    }
                    return;
                }
            }
            else if (states == ReversiCell.ReversiCellStates.Black)
            {
                if (reversiCells[x + 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    RTturnOverList.Add(reversiCells[x + 1, y + 1]);
                    SRightTopCheck(x + 1, y + 1, states);
                }
                else if (reversiCells[x + 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    RTturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in RTturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.Black;
                        cell.m_anim.Play("BlackAnimation");
                    }
                    return;
                }
            }
        }    
    }

    public static void STopCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (y < m_rows - 1)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    TturnOverList.Add(reversiCells[x, y + 1]);
                    STopCheck(x, y + 1, states);
                }
                else if (reversiCells[x, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    TturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in TturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.White;
                        cell.m_anim.Play("WhiteAnimation");
                    }
                    return;
                }
            }
            else if (states == ReversiCell.ReversiCellStates.Black)
            {
                if (reversiCells[x, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    TturnOverList.Add(reversiCells[x, y + 1]);
                    STopCheck(x, y + 1, states);
                }
                else if (reversiCells[x, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    TturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in TturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.Black;
                        cell.m_anim.Play("BlackAnimation");
                    }
                    return;
                }
            }
        }
    }

    public static void SLeftTopCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (x > 0 && y < m_rows - 1)
        {
            if (states == ReversiCell.ReversiCellStates.White)
            {
                if (reversiCells[x - 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    LTturnOverList.Add(reversiCells[x - 1, y + 1]);
                    SLeftTopCheck(x - 1, y + 1, states);
                }
                else if (reversiCells[x - 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    LTturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in LTturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.White;
                        cell.m_anim.Play("WhiteAnimation");
                    }
                    return;
                }
            }
            else if (states == ReversiCell.ReversiCellStates.Black)
            {
                if (reversiCells[x - 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    LTturnOverList.Add(reversiCells[x - 1, y + 1]);
                    SLeftTopCheck(x - 1, y + 1, states);
                }
                else if (reversiCells[x - 1, y + 1].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    LTturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in LTturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.Black;
                        cell.m_anim.Play("BlackAnimation");
                    }
                    return;
                }
            }
        }    
    }

    public static void SLeftCheck(int x, int y, ReversiCell.ReversiCellStates states)
    {
        if (states == ReversiCell.ReversiCellStates.White)
        {
            if (x > 0)
            {
                if (reversiCells[x - 1, y].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                {
                    LturnOverList.Add(reversiCells[x - 1, y]);
                    SLeftCheck(x - 1, y, states);
                }
                else if (reversiCells[x - 1, y].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    LturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in LturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.White;
                        cell.m_anim.Play("WhiteAnimation");
                    }
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
                    LturnOverList.Add(reversiCells[x - 1, y]);
                    SLeftCheck(x - 1, y, states);
                }
                else if (reversiCells[x - 1, y].ReversiCellState == ReversiCell.ReversiCellStates.None)
                {
                    LturnOverList.Clear();
                }
                else
                {
                    foreach (var cell in LturnOverList)
                    {
                        cell.ReversiCellState = ReversiCell.ReversiCellStates.Black;
                        cell.m_anim.Play("BlackAnimation");
                    }
                    return;
                }
            }
        }
    }
}
