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
    [SerializeField] TurnState m_turnState = TurnState.WhiteTurn;
    [SerializeField] Text m_turnText = null;
    [SerializeField] Text m_whiteCellNumText = null;
    [SerializeField] Text m_blackCellNumText = null;
    ReversiCell[,] reversiCells = new ReversiCell[m_columns, m_rows];
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
        for (int i= 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                var cell = Instantiate(m_ReversiCellPrefab);
                cell.transform.SetParent(m_parent.transform);
                reversiCells[n, i] = cell;
                cell.transform.position = new Vector3( n, 0, i);
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
                WhiteThinking();
                break;
            case TurnState.BlackTurn:
                BlackThinking();
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
                if (reversiCells[n, i].ReversiCellState == ReversiCell.ReversiCellStates.White)
                {

                }
            }
        }
    }

    public void BlackThinking()
    {

    }

    public void PutCell()
    {
    }
}
