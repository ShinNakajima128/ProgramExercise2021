using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeGameSystem : MonoBehaviour
{
    /// <summary> X座標の数値 </summary>
    public static int m_columns = 128;
    /// <summary> Y座標の数値 </summary>
    public static int m_rows = 72;
    /// <summary> ライフゲームのセルのPrefab </summary>
    [SerializeField] LifeGameCell m_lifeGameCellPrefab = null;
    /// <summary> 生きているセルを表示するText </summary>
    [SerializeField] Text m_aliveCells;
    /// <summary> 世代を表示するText </summary>
    [SerializeField] Text m_generationText = null;
    /// <summary> 再生、停止を行うボタンのText </summary>
    [SerializeField] Text m_playButtonText = null;
    /// <summary> ライフゲームの全セルの配列 </summary>
    public static LifeGameCell[,] lifecells;
    /// <summary> タイマー </summary>
    public static float m_timer = 0;
    /// <summary> セルのステータスを決定するための数値 </summary>
    int cellstateNum = 0;
    /// <summary> 全セルの数 </summary>
    public static int allCells;
    /// <summary> 世代の数値 </summary>
    int generationNum = 1;
    /// <summary> 再生中かどうかの状態 </summary>
    bool isPlayed = true;
    SoundManager soundManager;

    private void Awake()
    {
        allCells = m_columns * m_rows;
    }

    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        soundManager.UnMuteBgm();
        lifecells = new LifeGameCell[m_columns, m_rows];

        ///セルを配置する。セルの状態も確率で決定する
        for (int i = 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                var cell = Instantiate(m_lifeGameCellPrefab);
                lifecells[n, i] = cell;
                cell.transform.position = new Vector3(-0.5f + n, -0.5f + i, 0);
                cellstateNum = Random.Range(0, 4);
                cell.cellNum = (i * 10) + n;

                if (cellstateNum > 0)
                {
                    cell.CellState = LifeGameCell.CellStates.dead;
                    allCells--;
                }
                else if (cellstateNum == 0)
                {
                    cell.CellState = LifeGameCell.CellStates.alive;
                }
            }
        }
    }

    void Update()
    {
        ///再生中だったら
        if (isPlayed)
        {
            LifeActivity();
            generationNum++;
        }

        /// 生存しているセルの数のTextを更新
        if (m_aliveCells != null) m_aliveCells.text = "生存数" + allCells.ToString() + "個";
        /// 世代を表示するTextを更新
        if (m_generationText != null) m_generationText.text = generationNum.ToString() + "世代";
    }

    /// <summary>
    /// 再生、停止を行う
    /// </summary>
    public void SetPlayState()
    {
        if (isPlayed)
        {
            soundManager.MuteBgm();
            isPlayed = false;
            m_playButtonText.text = "プレイ";
            m_playButtonText.color = Color.green;
        }
        else
        {
            soundManager.UnMuteBgm();
            isPlayed = true;
            m_playButtonText.text = "ストップ";
            m_playButtonText.color = Color.red;
        }
    }

    /// <summary>
    /// 1世代進める
    /// </summary>
    public void OneAdvanceGeneration()
    {
        soundManager.PlaySeByName("OneGene");
        LifeActivity();
        generationNum++;
    }

    /// <summary>
    /// セルの生命活動
    /// </summary>
    void LifeActivity()
    {
        for (int i = 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                GetNeighborCells(n, i, lifecells[n, i]);
            }
        }
    }

    /// <summary>
    /// 対象のセルの周りのセルを確認する
    /// </summary>
    /// <param name="x"> 対象のセルのX座標 </param>
    /// <param name="y"> 対象のセルのX座標 </param>
    /// <param name="cell"> 対象のセル </param>
    void GetNeighborCells(int x, int y, LifeGameCell cell)
    {
        cell.neighborCells = 0;

        ///左上
	    if (x == 0 && y == 0)
        {
            if (lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x + 1, y + 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
        }
        ///左下
        else if (x == 0 && y == m_rows - 1)
        {
            if (lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x + 1, y - 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
        }
        ///左端
        else if (x == 0 && y > 0 && y < m_rows - 1)
        {
            if (lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x + 1, y - 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x + 1, y + 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
        }
        ///右上
        else if (x == m_columns - 1 && y == 0)
        {
            if (lifecells[x - 1, y + 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
        }
        ///右下
        else if (x == m_columns - 1 && y == m_rows - 1)
        {
            if (lifecells[x - 1, y - 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
        }
        ///右端
        else if (x == m_columns - 1 && y > 0 && y < m_rows - 1)
        {
            if (lifecells[x - 1, y + 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x - 1, y - 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
        }
        ///端にない場合
        else if (x > 0 && x < m_columns - 1 && y > 0 && y < m_rows - 1)
        {
            if (lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x + 1, y - 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x - 1, y - 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x + 1, y + 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x - 1, y + 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
        }
        ///一番上の列
        else if (x > 0 && x < m_columns - 1 && y == 0)
        {
            if (lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x + 1, y + 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x - 1, y + 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x, y + 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
        }
        ///一番下の列
        else if (x > 0 && x < m_columns - 1 && y == m_rows - 1)
        {
            if (lifecells[x + 1, y].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x - 1, y].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x + 1, y - 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x - 1, y - 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
            if (lifecells[x, y - 1].CellState == LifeGameCell.CellStates.alive) cell.neighborCells++;
        }
    }

    /// <summary>
    /// Sceneを再読み込みする
    /// </summary>
    /// <param name="sceneName"></param>
    public void GameRetry(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
