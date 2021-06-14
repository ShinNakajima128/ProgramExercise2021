using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversiCell : MonoBehaviour
{
    [SerializeField] GameObject m_whiteCell = null;
    [SerializeField] GameObject m_blackCell = null;
    [SerializeField] GameObject m_board = null;
    [SerializeField] ReversiCellStates m_reversiCellStates = ReversiCellStates.None;
    public bool isWhitePlaceable = false;
    public bool isBlackPlaceable = false;
    public int m_cell_X = 0;
    public int m_cell_Y = 0;
    public Animator m_anim;
    SoundManager soundManager;

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

    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        m_anim = GetComponent<Animator>();
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

        if (isWhitePlaceable)
        {
            m_board.GetComponent<Renderer>().material.color = new Color(0.8f, 0.8f, 0.8f);
        }
        else if (isBlackPlaceable)
        {
            m_board.GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 0.4f);
        }
        else
        {
            m_board.GetComponent<Renderer>().material.color = new Color(0, 0.6f, 0.04f);
        }
    }

    void OnMouseDown()
    {
        Debug.Log(m_cell_X + "," + m_cell_Y);
        if (ReversiSystem.reversiCells[m_cell_X, m_cell_Y].isWhitePlaceable)
        {
            isWhitePlaceable = false;
            ReversiSystem.reversiCells[m_cell_X, m_cell_Y].ReversiCellState = ReversiCellStates.White;
            ReversiSystem.m_turnState = ReversiSystem.TurnState.BlackTurn;
            ReversiSystem.AddTurnOverCells(m_cell_X, m_cell_Y, ReversiCellState);
            TurnOver();
            ReversiSystem.isChecked = true;
        }
        else if (ReversiSystem.reversiCells[m_cell_X, m_cell_Y].isBlackPlaceable)
        {
            isBlackPlaceable = false;
            ReversiSystem.reversiCells[m_cell_X, m_cell_Y].ReversiCellState = ReversiCellStates.Black;
            ReversiSystem.m_turnState = ReversiSystem.TurnState.WhiteTurn;
            ReversiSystem.AddTurnOverCells(m_cell_X, m_cell_Y, ReversiCellState);
            TurnOver();
            ReversiSystem.isChecked = true;
        }
    }

    public void TurnOver()
    {
        if (ReversiSystem.m_turnState == ReversiSystem.TurnState.WhiteTurn)
        {
            foreach (var cell in ReversiSystem.PlaceableList)
            {
                cell.isWhitePlaceable = false;
                cell.isBlackPlaceable = false;
            }
        }
        else if (ReversiSystem.m_turnState == ReversiSystem.TurnState.BlackTurn)
        {
            foreach (var cell in ReversiSystem.PlaceableList)
            {
                cell.isWhitePlaceable = false;
                cell.isBlackPlaceable = false;
            }
        } 
        ReversiSystem.PlaceableList.Clear();
    }

    public void TurnOverSE()
    {
        soundManager.PlaySeByName("Turnover");
    }
    public void LandingSE()
    {
        soundManager.PlaySeByName("Landing");
    }
}
