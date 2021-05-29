using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGameCell : MonoBehaviour
{
    [SerializeField] CellStates m_cellStates;
    [SerializeField] GameObject m_deadCell = null;
    [SerializeField] GameObject m_aliveCell = null;
    public int cellNum = 0;
    public int NeighborCells = 0;
    int m_rows = 36;
    int m_columns = 64;

    public CellStates CellState
    {
        get => m_cellStates;
        set
        {
            m_cellStates = value;
        }
    }

    public enum CellStates
    {
        dead,
        alive
    }

    void awake()
    {
        m_cellStates = CellStates.dead;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LifeActivity(this);
        }
        //LifeActivity(this);

        //if (CellState == CellStates.alive)
        //{
        //    if (NeighborCells <= 1 || NeighborCells >= 4)
        //    {
        //        CellState = CellStates.dead;
        //    }
        //    else if (NeighborCells == 2 || NeighborCells == 3)
        //    {
        //        CellState = CellStates.alive;
        //    }
        //}
        //else if (CellState == CellStates.dead && NeighborCells == 3)
        //{
        //    CellState = CellStates.alive;
        //}

        switch (m_cellStates)
        {
            case CellStates.dead:
                m_deadCell.SetActive(true);
                m_aliveCell.SetActive(false);
                break;
            case CellStates.alive:
                m_deadCell.SetActive(false);
                m_aliveCell.SetActive(true);            
                break;
        }


    }

    private void CellStateChange()
    {
        if (CellState == CellStates.alive && NeighborCells <= 1)
        {
            CellState = CellStates.dead;
            LifeGameSystem.allCells--;
        }
        else if (CellState == CellStates.alive && NeighborCells >= 4)
        {
            CellState = CellStates.dead;
            LifeGameSystem.allCells--;
        }
        else if (CellState == CellStates.dead && NeighborCells == 3)
        {
            CellState = CellStates.alive;
            LifeGameSystem.allCells++;
        }
        else if (CellState == CellStates.alive && NeighborCells == 2)
        {
            CellState = CellStates.alive;
            //LifeGameSystem.allCells++;
        }
        else if (CellState == CellStates.alive && NeighborCells == 3)
        {
            CellState = CellStates.alive;
        }
    }

    void LifeActivity(LifeGameCell cell)
    {
        for (int i = 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                if (LifeGameSystem.lifecells[n, i].cellNum == cell.cellNum)
                {
                    GetNeighborCells(n, i);
                    break;
                }
            }
        }
    }

    void GetNeighborCells(int x, int y)
    {
        NeighborCells = 0;

        if (x == 0 && y == 0)
        {
            if (LifeGameSystem.lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x + 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;

        }
        else if (x == 0 && y == m_rows - 1)
        {
            if (LifeGameSystem.lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x + 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;

        }
        else if (x == 0 && y > 0 && y < m_rows - 1)
        {
            if (LifeGameSystem.lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x + 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x + 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;


        }
        else if (x == m_columns - 1 && y == 0)
        {
            if (LifeGameSystem.lifecells[x - 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;


        }
        else if (x == m_columns - 1 && y == m_rows - 1)
        {
            if (LifeGameSystem.lifecells[x - 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;


        }
        else if (x == m_columns - 1 && y > 0 && y < m_rows - 1)
        {
            if (LifeGameSystem.lifecells[x - 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x - 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;


        }
        else if (x > 0 && x < m_columns - 1 && y > 0 && y < m_rows - 1)
        {
            if (LifeGameSystem.lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x + 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x - 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x + 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x - 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;


        }
        else if (x > 0 && x < m_columns - 1 && y == 0)
        {
            if (LifeGameSystem.lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x + 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x - 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;


        }
        else if (x > 0 && x < m_columns - 1 && y == m_rows - 1)
        {
            if (LifeGameSystem.lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x + 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x - 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (LifeGameSystem.lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;

        }

        CellStateChange();
    }
}
