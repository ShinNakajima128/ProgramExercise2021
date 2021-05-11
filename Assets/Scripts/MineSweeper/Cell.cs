using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] Text m_view = null;
    [SerializeField] CellStates m_cellStates = CellStates.None;

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


    private void OnValidate()
    {
        OnCellStateChanged();
    }

    private void OnCellStateChanged()
    {
        if (m_view == null) return;

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
