using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    /// <summary> Cellのステータスを表示するText </summary>
    [SerializeField] Text m_view = null;
    /// <summary> Cellのステータス </summary>
    [SerializeField] CellStates m_cellStates = CellStates.None;
    /// <summary> CellのImage　</summary>
    [SerializeField] Image m_bg = null;
    /// <summary> Cellを探すためのIDナンバー </summary>
    public int m_indexNum = 0;
    /// <summary> Cellの開閉状態 </summary>
    public bool isOpened = false;
    /// <summary> 旗が立っているかどうかの状態 </summary>
    bool isFlagged = false;
    /// <summary> 一番最初のCellを押したかどうかの状態 </summary>
    public static bool isFirstSerected = true;

    /// <summary>
    /// Cellのステータス
    /// </summary>
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

    /// <summary>
    /// Cellのステータスを更新する
    /// </summary>
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

    /// <summary>
    /// Cellを開ける
    /// </summary>
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

    /// <summary>
    /// 旗を立てる、または除去する
    /// </summary>
    public void Flag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isOpened) return;

            if (isFlagged)
            {
                Debug.Log("旗を取り除きました");
                m_bg.color = new Color(0, 1, 1);
                m_view.text = "";
                isFlagged = false;
            }
            else
            {
                Debug.Log("旗を立てました");
                m_bg.color = new Color(1, 0, 0);
                m_view.text = "M";
                m_view.color = Color.yellow;
                isFlagged = true;
            } 
        }
    }
}
