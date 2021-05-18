using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineSweeperSystem : MonoBehaviour
{
    [SerializeField] static  int m_fieldSizeX = 10;             
    [SerializeField] static int m_fieldSizeY = 10;
    [SerializeField] GameObject m_cellPrefab = null;
    [SerializeField] Transform m_cellField = null;
    [SerializeField] int m_mineAmount = 20;
    Cell m_cellStates;
    public static GameObject[,] fieldCellObjects;

    void Start()
    {
        fieldCellObjects = new GameObject[m_fieldSizeY, m_fieldSizeX];

        for (int i = 0; i < m_fieldSizeY; i++)
        {
            for (int n = 0; n < m_fieldSizeX; n++)
            {
                var cell = Instantiate(m_cellPrefab);
                fieldCellObjects[n, i] = cell;

                m_cellStates = cell.GetComponent<Cell>();

                cell.transform.SetParent(m_cellField);
                cell.transform.position = new Vector3(-0.5f + n, 0, -0.5f + i);

                if (m_mineAmount > 0)
                {
                    m_cellStates.CellState = (Cell.CellStates)(Random.Range(-1,1));

                    if (m_cellStates.CellState == (Cell.CellStates)(-1))
                    {
                        m_mineAmount--;
                    }
                }
            }
        }
    }

    void Update()
    {
        
    }
}
