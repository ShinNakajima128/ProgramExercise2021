using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineSweeperSystem : MonoBehaviour
{
    [SerializeField] int m_fieldSizeX = 10;             
    [SerializeField] int m_fieldSizeY = 10;
    [SerializeField] GameObject m_cellPrefab = null;
    [SerializeField] Transform m_field = null;
    [SerializeField] int m_mineAmount = 20;
    Cell m_cellStates;
    GameObject[,] fieldCellObjects;


    void Start()
    {
        fieldCellObjects = new GameObject[m_fieldSizeY, m_fieldSizeX];

        for (int i = 0; i < m_fieldSizeY; i++)
        {
            for (int n = 0; n < m_fieldSizeX; n++)
            {
                var cell = Instantiate(m_cellPrefab);
                m_cellStates = cell.GetComponent<Cell>();
                fieldCellObjects[n, i] = cell;
                cell.transform.SetParent(m_field, false);
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

        for (int i = 0;i < m_fieldSizeY; i++)
        {
            for (int n = 0; n < m_fieldSizeX; n++)
            {
                m_cellStates = fieldCellObjects[n, i].GetComponent<Cell>();

                if (m_cellStates.CellState != (Cell.CellStates)(-1))
                {

                }
            }
        }
    }

    void Update()
    {
        
    }

    
}
