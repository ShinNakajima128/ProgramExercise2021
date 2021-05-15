using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineSweeperSystem : MonoBehaviour
{
    [SerializeField] int m_fieldSizeX = 10;             
    [SerializeField] int m_fieldSizeY = 10;
    [SerializeField] GameObject m_cellPrefab = null;
    [SerializeField] GameObject m_coverPrefab = null;
    [SerializeField] Transform m_cellField = null;
    [SerializeField] Transform m_coverField = null;
    [SerializeField] int m_mineAmount = 20;
    [SerializeField] Image m_coverImage = null;
    GameObject targetCover;
    Cell m_cellStates;
    Cover m_coverStates;
    RaycastHit hit;
    GameObject[,] fieldCellObjects;
    public GameObject[,] fieldCoverObjects;


    void Start()
    {
        fieldCellObjects = new GameObject[m_fieldSizeY, m_fieldSizeX];

        for (int i = 0; i < m_fieldSizeY; i++)
        {
            for (int n = 0; n < m_fieldSizeX; n++)
            {
                var cell = Instantiate(m_cellPrefab);
                var cover = Instantiate(m_coverPrefab);

                m_cellStates = cell.GetComponent<Cell>();
                m_coverStates = cover.GetComponent<Cover>();
                fieldCellObjects[n, i] = cell;
                fieldCoverObjects[n, i] = cover;

                cell.transform.SetParent(m_cellField);
                cell.transform.position = new Vector3(-0.5f + n, 0, -0.5f + i);

                cover.transform.SetParent(m_coverField);
                cover.transform.position = new Vector3(-0.5f + n, 0, -0.5f + i);

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
        SelectCover();
    }

    void SelectCover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            targetCover = hit.collider.gameObject;
            targetCover.GetComponent<Cover>().CoverState = Cover.CoverStates.Select;

            Debug.Log(targetCover);
            if (m_coverImage.enabled && Input.GetMouseButtonDown(0))
            {
                targetCover.GetComponent<Cover>().CoverState = Cover.CoverStates.Open;
                Debug.Log(targetCover);
            }
        }
        else
        {
            if (!targetCover) return;
            targetCover.GetComponent<Cover>().CoverState = Cover.CoverStates.Close;
            Debug.Log(targetCover);
        }
    }

    public void CheckCovers(GameObject cover) 
    {
        for (int i = 0; i < m_fieldSizeY; i++)
        {
            for (int n = 0; n < m_fieldSizeX; n++)
            {
                Cover CheckCover = cover.GetComponent<Cover>();


            }
        }
    }
}
