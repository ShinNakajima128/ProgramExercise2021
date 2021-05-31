using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BingoSystem : MonoBehaviour
{
    [SerializeField]  int m_columns = 5;
    [SerializeField]  int m_rows = 5;
    [SerializeField] GridLayoutGroup m_gridLayoutGroup = null;
    [SerializeField] BingoCell m_cellPrefab = null;
    [SerializeField] Text m_lotNumText = null;
    public BingoCell[,] Bingocells;
    const int index = 25;
    int[] bingoCellElements = new int[index];
    int lotNum = 0;
    int[] lottedNums = new int[70];
    int lottedIndex = 0;

    void Start()
    {
        var parent = m_gridLayoutGroup.transform;
        Bingocells = new BingoCell[m_columns, m_rows];

        for (int n = 0; n < index; n++)
        {
            ElementInsert(n);
            Debug.Log(bingoCellElements[n]);
        }

        if (m_columns < m_rows)
        {
            m_gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            m_gridLayoutGroup.constraintCount = m_columns;
        }
        else
        {
            m_gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            m_gridLayoutGroup.constraintCount = m_rows;
        }

        for (int i = 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                var cell = Instantiate(m_cellPrefab);
                cell.transform.SetParent(parent);
                Bingocells[n, i] = cell;
                if (i == 2 && n == 2) continue;
                cell.CellNum = bingoCellElements[i * m_rows + n];
            }
        }
    }

    void Update()
    {
        if (m_lotNumText != null) m_lotNumText.text = lotNum.ToString();
    }

    public void ElementInsert(int x)
    {
        bingoCellElements[x] = Random.Range(1, 71);

        if (ElementCheck(x,bingoCellElements))
        {
            return;
        }
        else
        {
            ElementInsert(x);
        }
    }

    public bool ElementCheck(int x, int[] CheckArray)
    {
        for (int i = x - 1; i >= 0; i--)
        {
            if (CheckArray[i] == CheckArray[x])
            {
                return false;
            }
            else
            {
                continue;
            }
        }
        return true;
    }

    public void LotteryNum()
    {
        lottedNums[lottedIndex] = Random.Range(1, 71);

        if (ElementCheck(lottedIndex, bingoCellElements))
        {
            return;
        }
        else
        {
            LotteryNum();
        }

        lottedIndex++;
    }


}
