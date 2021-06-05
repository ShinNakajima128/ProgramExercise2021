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
                else if(reversiCells[n, i].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {
                    CheckCells(n, i, reversiCells[n, i].ReversiCellState);
                }
            }
        }
    }

    public void BlackThinking()
    {

    }

    public void CheckCells(int x, int y, ReversiCell.ReversiCellStates states)
    {
        int bottom = y - 1;
        int top = y + 1;
        int right = x + 1;
        int left = x - 1;

        if (states == ReversiCell.ReversiCellStates.White)
        {
            if (x > 0)
            {
                if (y > 0)
                {
                    if (reversiCells[left, bottom].ReversiCellState == ReversiCell.ReversiCellStates.White)
                    {
                        return;
                    }
                    else if (reversiCells[left, bottom].ReversiCellState == ReversiCell.ReversiCellStates.Black)
                    {
                        LeftBottomCheck(left, bottom);
                    }
                }
            }
        }
        else if (reversiCells[x, y].ReversiCellState == ReversiCell.ReversiCellStates.Black)
        {

        }
    }

    public void LeftBottomCheck(int x, int y)
    {
        if (reversiCells[x - 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.Black)
        {
            LeftBottomCheck(x - 1, y - 1);
        }
        else if (reversiCells[x - 1, y - 1].ReversiCellState == ReversiCell.ReversiCellStates.White)
        {

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
