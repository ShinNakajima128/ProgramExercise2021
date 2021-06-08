using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] Text m_view = null;
    [SerializeField] CellStates m_cellStates = CellStates.None;
    [SerializeField] Image m_bg = null;
    public int m_indexNum = 0;
    public bool isOpened = false;
    bool isFlaged = false;
    public static bool isFirstSerected = true;

    public CellStates CellState
    {
        get => m_cellStates;
        set
        {
            m_cellStates = value;
            OnCellStateChanged();
        }
    }
    public enum CellStates
    {
        None = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,

        Mine = -1,
    }

    private void Start()
    {
        m_bg.color = new Color(0, 1, 1);
    }

    private void OnValidate()
    {
        OnCellStateChanged();
    }

    void OnCellStateChanged()
    {
        if (m_view == null) return;

        if (MineSweeper.InGame)
        {
            if (isOpened)
            {
                if (m_cellStates == CellStates.None)
                {
                    m_view.text = "";
                }
                else if (m_cellStates == CellStates.Mine)
                {
                    m_view.text = "X";
                    m_view.color = Color.red;
                }
                else
                {
                    m_view.text = ((int)m_cellStates).ToString();
                    m_view.color = Color.blue;
                }
            }
        }    
    }

    public void Open()
    {
        isOpened = true;

        if (isFirstSerected)
        {
            MineSweeper.SetMine();
            isFirstSerected = false;
        }
        else
        {
            if (CellState == CellStates.Mine) { MineSweeper.InGame = false; }
        }

        MineSweeper._closeCellCount--;
        MineSweeper.CheckCells(this);
        OnCellStateChanged();
        m_bg.color = new Color(1, 1, 1);
    }

    public void Flag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isOpened) return;

            if (isFlaged)
            {
                Debug.Log("旗を取り除きました");
                m_bg.color = new Color(0, 1, 1);
                m_view.text = "";
                isFlaged = false;
            }
            else
            {
                Debug.Log("旗を立てました");
                m_bg.color = new Color(1, 0, 0);
                m_view.text = "M";
                m_view.color = Color.yellow;
                isFlaged = true;
            } 
        }
    }
}
