using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGameCell : MonoBehaviour
{
    /// <summary> セルの状態 </summary>
    [SerializeField] CellStates m_cellStates;
    /// <summary> 死んでいるセル </summary>
    [SerializeField] GameObject m_deadCell = null;
    /// <summary> 生きているセル </summary>
    [SerializeField] GameObject m_aliveCell = null;
    /// <summary> Cellを探す時用のNumber </summary>
    public int cellNum = 0;
    /// <summary> 周りの生きているセルの数 </summary>
    public int neighborCells = 0;
    
    public CellStates CellState
    {
        get => m_cellStates;
        set
        {
            m_cellStates = value;
        }
    }

    public enum CellStates
    {
        dead,
        alive
    }

    void LateUpdate()
    {
        if (LifeGameSystem.isPlayed) CellStateChange();

        switch (m_cellStates)
        {
            case CellStates.dead:
                m_deadCell.SetActive(true);
                m_aliveCell.SetActive(false);
                break;
            case CellStates.alive:
                m_deadCell.SetActive(false);
                m_aliveCell.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// セルの状態を更新する
    /// </summary>
    public void CellStateChange()
    {
        ///自分は生きている&周りの生きているセルが1つ以下
        if (CellState == CellStates.alive && neighborCells <= 1)
        {
            CellState = CellStates.dead;
            LifeGameSystem.allCells--;
        }
        ///自分は生きている&周りの生きているセルが4つ以上
        else if (CellState == CellStates.alive && neighborCells >= 4)
        {
            CellState = CellStates.dead;
            LifeGameSystem.allCells--;
        }
        ///自分は死んでいる&周りの生きているセルが3つ
        else if (CellState == CellStates.dead && neighborCells == 3)
        {
            CellState = CellStates.alive;
            LifeGameSystem.allCells++;
        }
        ///自分は生きている&周りの生きているセルが2つ
        else if (CellState == CellStates.alive && neighborCells == 2)
        {
            return;
        }
        ///自分は生きている&周りの生きているセルが3つ
        else if (CellState == CellStates.alive && neighborCells == 3)
        {
            return;
        }
        ///その他
        else
        {
            if (CellState == CellStates.alive) LifeGameSystem.allCells--;
            CellState = CellStates.dead;
        }
    }

   
}
