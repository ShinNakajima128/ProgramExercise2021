using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversiCell : MonoBehaviour
{
    [SerializeField] GameObject m_whiteCell = null;
    [SerializeField] GameObject m_blackCell = null;
    [SerializeField] GameObject m_board = null;
    [SerializeField] ReversiCellStates m_reversiCellStates = ReversiCellStates.None;
    public bool isPlaceable = false;
    public int m_cell_X = 0;
    public int m_cell_Y = 0;

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

    void Update()
    {
        switch (m_reversiCellStates)
        {
            case ReversiCellStates.None:
                m_whiteCell.SetActive(false);
                m_blackCell.SetActive(false);
                break;
            case ReversiCellStates.Black:
                m_whiteCell.SetActive(false);
                m_blackCell.SetActive(true);
                break;
            case ReversiCellStates.White:
                m_whiteCell.SetActive(true);
                m_blackCell.SetActive(false);
                break;
        }

        if (isPlaceable)
        {
            m_board.GetComponent<Renderer>().material.color = new Color(0, 0.8f, 0.6f);
        }
        else
        {
            m_board.GetComponent<Renderer>().material.color = new Color(0, 0.6f, 0.04f);
        }
    }

    void OnMouseDown()
    {
        Debug.Log(m_cell_X + "," + m_cell_Y);

        
    }
}
