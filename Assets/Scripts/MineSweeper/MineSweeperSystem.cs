using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperSystem : MonoBehaviour
{
    [SerializeField] int m_fieldSizeX = 10;             
    [SerializeField] int m_fieldSizeY = 10;
    [SerializeField] GameObject m_cellPrefab = null;
    
    /// <summary>
    /// Cellの状態
    /// </summary>
    public enum CellStates
    {
        None,
        Default,    //開いてない状態
        Open        //開いてる状態
    }

    void Start()
    {
        GameObject[,] fieldCellObjects = new GameObject[m_fieldSizeY, m_fieldSizeX];

        for (int i = 0; i < m_fieldSizeY; i++)
        {
            for (int n = 0; n < m_fieldSizeX; n++)
            {
                var cell = Instantiate(m_cellPrefab);
                fieldCellObjects[n, i] = cell;
                cell.transform.position = new Vector3(-0.5f + n, 0, -0.5f + i);
            }
        }
    }

    void Update()
    {
        
    }

    
}
