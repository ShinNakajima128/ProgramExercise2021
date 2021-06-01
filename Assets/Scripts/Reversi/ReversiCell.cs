using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversiCell : MonoBehaviour
{
    [SerializeField] GameObject m_whiteCell = null;
    [SerializeField] GameObject m_blackCell = null;
    [SerializeField] GameObject m_board = null;
    [SerializeField] ReversiCellStates m_reversiCellStates = ReversiCellStates.None;

    public enum ReversiCellStates
    {
        None,
        White,
        Black
    }

    public ReversiCellStates ReversiCellState
    {
        get => m_reversiCellStates;
        set
        {
            m_reversiCellStates = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_reversiCellStates)
        {
            case ReversiCellStates.None:
                m_whiteCell.SetActive(false);
                m_blackCell.SetActive(false);
                break;
            case ReversiCellStates.Black:
                m_whiteCell.SetActive(true);
                m_blackCell.SetActive(false);
                break;
            case ReversiCellStates.White:
                m_whiteCell.SetActive(false);
                m_blackCell.SetActive(true);
                break;
        }
    }
}
