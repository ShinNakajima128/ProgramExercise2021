using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BingoCell : MonoBehaviour
{
    [SerializeField] Text m_view = null;
    [SerializeField] Image m_bg = null;
    BingoCellStates bingoCellState = BingoCellStates.Close;
    public bool isCellOpened = false;
    public int CellNum = 0;

    public BingoCellStates BingoCellState
    {
        get => bingoCellState;
        set
        {
            bingoCellState = value;
        }
    }

    public enum BingoCellStates
    {
        Close,
        Open
    }

    void Start()
    {
        m_bg.color = new Color(0.3f, 0.3f, 0.3f);
        if (CellNum == 0)
        {
            m_view.text = "Bingo";
        }
        else
        {
            m_view.text = CellNum.ToString();
        }
    }

    void Update()
    {
        if (m_view == null) return;

        if (isCellOpened)
        {
            m_bg.color = new Color(1, 0, 0);
        }
    }
}
