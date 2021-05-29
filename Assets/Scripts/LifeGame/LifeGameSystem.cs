using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeGameSystem : MonoBehaviour
{
    public static int m_columns = 64;
    public static int m_rows = 36;
    [SerializeField] LifeGameCell m_lifeGameCellPrefab = null;
    [SerializeField] Text m_aliveCells;
    [SerializeField] Text m_generationText = null;
    public static LifeGameCell[,] lifecells;
    int cellstateNum = 0;
    public static int allCells;
    int generationNum = 1;

    private void Awake()
    {
        allCells = m_columns * m_rows;
    }
    void Start()
    {
        lifecells = new LifeGameCell[m_columns, m_rows];

        for (int i = 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                var cell = Instantiate(m_lifeGameCellPrefab);
                lifecells[n, i] = cell;
                cell.transform.position = new Vector3(-0.5f + n, -0.5f + i, 0);
                cellstateNum = Random.Range(0, 5);
                cell.cellNum = (i * 10) + n;

                if (cellstateNum > 0)
                {
                    cell.CellState = LifeGameCell.CellStates.dead;
                    allCells--;
                }
                else if (cellstateNum == 0)
                {
                    cell.CellState = LifeGameCell.CellStates.alive;
                    //allCells++;
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) generationNum++;
        if (m_aliveCells != null) m_aliveCells.text = "生存しているセル" + allCells.ToString() + "個";
        if (m_generationText != null) m_generationText.text = generationNum.ToString() + "世代";
    }

    public static void LifeActivity(LifeGameCell cell)
    {
        for (int i = 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                if (lifecells[n, i].cellNum == cell.cellNum)
                {
                    GetNeighborCells(i, n, cell.NeighborCells);
                    break;
                }
            }
        }
    }
    public static void GetNeighborCells(int x, int y, int NeighborCells)
    {
        NeighborCells = 0;

	    if (x == 0 && y == 0)
        {
            if (lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x + 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;

        }
        else if (x == 0 && y == m_rows - 1)
        {
            if (lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x + 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;

        }
        else if (x == 0 && y > 0 && y < m_rows - 1)
        {
            if (lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x + 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x + 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;


        }
        else if (x == m_columns - 1 && y == 0)
        {
            if (lifecells[x - 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;


        }
        else if (x == m_columns - 1 && y == m_rows - 1)
        {
            if (lifecells[x - 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;


        }
        else if (x == m_columns - 1 && y > 0 && y < m_rows - 1)
        {
            if (lifecells[x - 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x - 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;


        }
        else if (x > 0 && x < m_columns - 1 && y > 0 && y < m_rows - 1)
        {
            if (lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x + 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x - 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x + 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x - 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;


        }
        else if (x > 0 && x < m_columns - 1 && y == 0)
        {
            if (lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x + 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x - 1, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;


        }
        else if (x > 0 && x < m_columns - 1 && y == m_rows - 1)
        {
            if (lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x + 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x - 1, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;
            if (lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) NeighborCells++;

        }
    }
}
