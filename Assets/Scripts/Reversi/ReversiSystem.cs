using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversiSystem : MonoBehaviour
{
    const int m_columns = 8;
    const int m_rows = 8;
    [SerializeField] ReversiCell m_ReversiCellPrefab = null;
    ReversiCell[,] reversiCells = new ReversiCell[m_columns, m_rows];

    void Start()
    {
        for (int i= 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                var cell = Instantiate(m_ReversiCellPrefab);
                reversiCells[n, i] = cell;
                cell.transform.position = new Vector3( n, 0, i);
                if (n == 3 && i == 3 || n == 4 && i == 4) cell.ReversiCellState = ReversiCell.ReversiCellStates.White;
                if (n == 4 && i == 3 || n == 3 && i == 4) cell.ReversiCellState = ReversiCell.ReversiCellStates.Black;
            }
        }
    }

    void Update()
    {
        
    }
}
