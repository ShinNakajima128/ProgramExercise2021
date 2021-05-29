using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGameCell : MonoBehaviour
{
    [SerializeField] CellStates m_cellStates;
    [SerializeField] GameObject m_deadCell = null;
    [SerializeField] GameObject m_aliveCell = null;
    public int cellNum = 0;
    public int neighborCells = 0;

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

    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            CellStateChange();
        }

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

     void CellStateChange()
    {
        if (CellState == CellStates.alive && neighborCells <= 1)
        {
            CellState = CellStates.dead;
            LifeGameSystem.allCells--;
        }
        else if (CellState == CellStates.alive && neighborCells >= 4)
        {
            CellState = CellStates.dead;
            LifeGameSystem.allCells--;
        }
        else if (CellState == CellStates.dead && neighborCells == 3)
        {
            CellState = CellStates.alive;
            LifeGameSystem.allCells++;
        }
        else if (CellState == CellStates.alive && neighborCells == 2)
        {
            return;
            //CellState = CellStates.alive;
            //LifeGameSystem.allCells++;
        }
        else if (CellState == CellStates.alive && neighborCells == 3)
        {
            return;
            //CellState = CellStates.alive;
        }
        else
        {
            if (CellState == CellStates.alive) LifeGameSystem.allCells--;
            CellState = CellStates.dead;
        }
    }
}
