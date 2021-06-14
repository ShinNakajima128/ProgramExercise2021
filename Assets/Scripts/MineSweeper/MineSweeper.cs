using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MineSweeper : MonoBehaviour
{
    /// <summary> X座標の数値 </summary>
    [SerializeField] static int _columns = 10;
    /// <summary> Y座標の数値 </summary>
    [SerializeField] static int _rows = 10;
    /// <summary> Cellを表示するエリア </summary>
    [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
    /// <summary> CellのPrefab </summary>
    [SerializeField] Cell _cellPrefab = null;
    /// <summary> ゲームオーバー時に表示するパネル </summary>
    [SerializeField] GameObject _gameoverPanel = null;
    /// <summary> クリアした時に表示するパネル </summary>
    [SerializeField] GameObject _gameclearPanel = null;
    /// <summary> 地雷の合計 </summary>
    [SerializeField] int _mineCount = 1;
    /// <summary> 地雷の合計を表示するText </summary>
    [SerializeField] Text _mineNumText = null;
    /// <summary> 旗の合計を表示するText </summary>
    [SerializeField] Text _flagNumText = null;
    /// <summary> プレイ時間を表示するText </summary>
    [SerializeField] Text _timerText = null;
    /// <summary> クリアタイムを表示するText </summary>
    [SerializeField] Text _clearTimeText = null;
    /// <summary> Cellの周りにある地雷の数 </summary>
    public static int aroundMine = 0;
    /// <summary> 全Cellの配列 </summary>
    public static Cell[,] _cells;
    /// <summary> ゲーム中か否か </summary>
    public static bool InGame = true;
    /// <summary> 開いていないCellの合計 </summary>
    public static int _closeCellCount;
    /// <summary> 地雷の合計 </summary>
    public static int _mineCountS;
    /// <summary> 旗の数 </summary>
    public static int flagNum = 0;
    /// <summary> プレイ時間のタイマー </summary>
    float m_timer;
    /// <summary> 秒の数値 </summary>
    int secondNum = 0;
    /// <summary> 分の数値 </summary>
    int minuteNum = 0;
    SoundManager soundManager;
    bool isDisplayed = false;


    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        soundManager.UnMuteBgm();
        InGame = true;
        _mineCountS = _mineCount;
        flagNum = 0;
        _closeCellCount = _columns * _rows;
        _gameoverPanel.SetActive(false);
        _gameclearPanel.SetActive(false);
        Cell.isFirstSerected = true;

        var parent = _gridLayoutGroup.transform;

        if (_columns < _rows)
        {
            _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayoutGroup.constraintCount = _columns;
        }
        else
        {
            _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            _gridLayoutGroup.constraintCount = _rows;
        }

        _cells = new Cell[_rows, _columns];

        ///Cellを配置する
        for (int i = 0; i < _rows; i++)
        {
            for (int n = 0; n < _columns; n++)
            {
                var cell = Instantiate(_cellPrefab);
                cell.transform.SetParent(parent);
                _cells[n, i] = cell;
                _cells[n, i].m_indexNum = (i * 10) + n;
            }
        }
    }
    private void Update()
    {
        ///開いていないcellと地雷の数が一緒ならゲーム終了
        if (_closeCellCount == _mineCount) InGame = false;

        if (InGame)
        {
            m_timer += Time.deltaTime;
            secondNum = (int)m_timer;
        }
        
        if (m_timer >= 60.0f)
        {
            minuteNum++;
            m_timer = 0f;
        }

        _mineNumText.text = "地雷の数：" + _mineCountS.ToString() + "個";
        _flagNumText.text = "旗の数：" + flagNum.ToString() + "個";
        _timerText.text = minuteNum.ToString("D2") + "：" + secondNum.ToString("D2");


        ///ゲーム終了後
        if (!InGame && !isDisplayed)
        {
            soundManager.MuteBgm();
            ///開いていないcellと地雷の数が一緒ならクリア画面を表示
            if (_closeCellCount == _mineCount)
            {
                soundManager.PlaySeByName("MineSweeped");
                AllCellOpen();
                _gameclearPanel.SetActive(true);
                _clearTimeText.text = "クリアタイム　" + minuteNum.ToString("D2") + "：" + secondNum.ToString("D2");
            }
            ///それ以外の時はゲームオーバー画面を表示
            else
            {
                
                Debug.Log("Called");
                soundManager.PlaySeByName("Explosion");
                AllCellOpen();
                _gameoverPanel.SetActive(true);
            }
            isDisplayed = true;
        }
    }

    /// <summary>
    /// 地雷を配置する
    /// </summary>
    public static void SetMine()
    {
        for (var i = 0; i < _mineCountS;)
        {
            var r = Random.Range(0, _rows);
            var c = Random.Range(0, _columns);
            var cell = _cells[r, c];

            if (cell.CellState == Cell.CellStates.Mine || cell.isOpened)
            {
                continue;
            }
            else
            {
                cell.CellState = Cell.CellStates.Mine;
                i++;
            }
        }
        StartCell();
    } 
    
    /// <summary>
    /// 全てのCellを開ける
    /// </summary>
    public void AllCellOpen()
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int n = 0; n < _columns; n++)
            {
                var cell = _cells[n, i];
                if (!_cells[n, i].isOpened)
                {
                    
                    if (cell.CellState == Cell.CellStates.Mine)
                    {
                        cell.isOpened = true;
                        cell.GetComponent<Image>().color = new Color(1, 1, 0);
                        cell.GetComponentInChildren <Text>().text = "X";
                        cell.GetComponentInChildren<Text>().color = Color.red;
                    }
                    else if (cell.CellState == Cell.CellStates.None)
                    {
                        cell.isOpened = true;
                        cell.GetComponentInChildren<Text>().text = "";
                    }
                    else
                    {
                        cell.isOpened = true;
                        cell.GetComponentInChildren<Text>().text = ((int)_cells[n, i].CellState).ToString();
                        cell.GetComponentInChildren<Text>().color = Color.blue;
                    }
                }
                else if (cell.isOpened)
                {
                    if (cell.CellState == Cell.CellStates.Mine)
                    {
                        cell.GetComponent<Image>().color = new Color(1, 1, 0);
                        cell.GetComponentInChildren<Text>().text = "X";
                        cell.GetComponentInChildren<Text>().color = Color.red;
                    }
                    else if (cell.CellState == Cell.CellStates.None)
                    {
                        cell.GetComponentInChildren<Text>().text = "";
                    }
                    else
                    {
                        cell.GetComponentInChildren<Text>().text = ((int)_cells[n, i].CellState).ToString();
                        cell.GetComponentInChildren<Text>().color = Color.blue;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 開始時にCellの状態を更新する
    /// </summary>
    public static void StartCell()
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int n = 0; n < _columns; n++)
            {
                var cell = _cells[n, i];
                aroundMine = 0;
                if (cell.CellState != Cell.CellStates.Mine)
                {
                    if (n == 0 && i == 0)
                    {
                        if (_cells[n + 1, i].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n + 1, i + 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n, i + 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        
                        cell.CellState = (Cell.CellStates)aroundMine;
                    }
                    else if (n == 0 && i == _rows - 1)
                    {
                        if (_cells[n + 1, i].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n, i - 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n + 1, i - 1].CellState == Cell.CellStates.Mine) aroundMine++;

                        cell.CellState = (Cell.CellStates)aroundMine;
                    }
                    else if (n == 0 && i > 0 && i < _rows - 1)
                    {
                        if (_cells[n, i - 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n + 1, i - 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n + 1, i + 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n, i + 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n + 1, i].CellState == Cell.CellStates.Mine) aroundMine++;

                        cell.CellState = (Cell.CellStates)aroundMine;
                    }
                    else if (n == _columns - 1 && i == 0)
                    {
                        if (_cells[n - 1, i + 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n - 1, i].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n, i + 1].CellState == Cell.CellStates.Mine) aroundMine++;

                        cell.CellState = (Cell.CellStates)aroundMine;
                    }
                    else if (n == _columns - 1 && i == _rows - 1)
                    {
                        if (_cells[n - 1, i - 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n - 1, i].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n, i - 1].CellState == Cell.CellStates.Mine) aroundMine++;

                        cell.CellState = (Cell.CellStates)aroundMine;
                    }
                    else if (n == _columns - 1 && i > 0 && i < _rows - 1)
                    {
                        if (_cells[n - 1, i + 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n - 1, i - 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n - 1, i].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n, i + 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n, i - 1].CellState == Cell.CellStates.Mine) aroundMine++;

                        cell.CellState = (Cell.CellStates)aroundMine;
                    }
                    else if (n > 0 && n < _columns - 1 && i > 0 && i < _rows - 1)
                    {
                        if (_cells[n + 1, i].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n - 1, i].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n, i - 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n + 1, i - 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n - 1, i - 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n + 1, i + 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n, i + 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n - 1, i + 1].CellState == Cell.CellStates.Mine) aroundMine++;

                        cell.CellState = (Cell.CellStates)aroundMine;
                    }
                    else if (n > 0 && n < _columns - 1 && i == 0)
                    {
                        if (_cells[n + 1, i].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n - 1, i].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n + 1, i + 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n - 1, i + 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n, i + 1].CellState == Cell.CellStates.Mine) aroundMine++;

                        cell.CellState = (Cell.CellStates)aroundMine;
                    }
                    else if (n > 0 && n < _columns - 1 && i == _rows - 1)
                    {
                        if (_cells[n + 1, i].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n - 1, i].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n + 1, i - 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n - 1, i - 1].CellState == Cell.CellStates.Mine) aroundMine++;
                        if (_cells[n, i - 1].CellState == Cell.CellStates.Mine) aroundMine++;

                        cell.CellState = (Cell.CellStates)aroundMine;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 選択したcellの周りのCellを調べる
    /// </summary>
    /// <param name="cell"> 選択したCell</param>
    public  static void CheckCells(Cell cell)
    {
        bool isChecked = false;

        for (int i = 0; i < _rows; i++)
        {
            for (int n = 0; n < _columns; n++)
            {
                if (cell.m_indexNum == _cells[n, i].m_indexNum && cell.CellState == Cell.CellStates.None && cell.isOpened)
                {
                    isChecked = true;
                    if (n == 0 && i == 0)
                    {
                        if (_cells[n + 1, i].CellState != Cell.CellStates.Mine && !_cells[n + 1, i].isOpened) _cells[n + 1, i].Open();
                        if (_cells[n + 1, i + 1].CellState != Cell.CellStates.Mine && !_cells[n + 1, i + 1].isOpened) _cells[n + 1, i + 1].Open();
                        if (_cells[n, i + 1].CellState != Cell.CellStates.Mine && !_cells[n, i + 1].isOpened) _cells[n, i + 1].Open();
                    }
                    else if (n == 0 && i == _rows - 1)
                    {
                        if (_cells[n + 1, i].CellState != Cell.CellStates.Mine && !_cells[n + 1, i].isOpened) _cells[n + 1, i].Open();
                        if (_cells[n, i - 1].CellState != Cell.CellStates.Mine && !_cells[n, i - 1].isOpened) _cells[n, i - 1].Open();
                        if (_cells[n + 1, i - 1].CellState != Cell.CellStates.Mine && !_cells[n + 1, i - 1].isOpened) _cells[n + 1, i - 1].Open();
                    }
                    else if (n == 0 && i > 0 && i < _rows - 1)
                    {
                        if (_cells[n, i - 1].CellState != Cell.CellStates.Mine && !_cells[n, i - 1].isOpened) _cells[n, i - 1].Open();
                        if (_cells[n + 1, i - 1].CellState != Cell.CellStates.Mine && !_cells[n + 1, i - 1].isOpened) _cells[n + 1, i - 1].Open();
                        if (_cells[n + 1, i + 1].CellState != Cell.CellStates.Mine && !_cells[n + 1, i + 1].isOpened) _cells[n + 1, i + 1].Open();
                        if (_cells[n, i + 1].CellState != Cell.CellStates.Mine && !_cells[n, i + 1].isOpened) _cells[n, i + 1].Open();
                        if (_cells[n + 1, i].CellState != Cell.CellStates.Mine && !_cells[n + 1, i].isOpened) _cells[n + 1, i].Open();
                    }
                    else if (n == _columns - 1 && i == 0)
                    {
                        if (_cells[n - 1, i + 1].CellState != Cell.CellStates.Mine && !_cells[n - 1, i + 1].isOpened) _cells[n - 1, i + 1].Open();
                        if (_cells[n - 1, i].CellState != Cell.CellStates.Mine && !_cells[n - 1, i].isOpened) _cells[n - 1, i].Open();
                        if (_cells[n, i + 1].CellState != Cell.CellStates.Mine && !_cells[n, i + 1].isOpened) _cells[n, i + 1].Open();
                    }
                    else if (n == _columns - 1 && i == _rows - 1)
                    {
                        if (_cells[n - 1, i - 1].CellState != Cell.CellStates.Mine && !_cells[n - 1, i - 1].isOpened) _cells[n - 1, i - 1].Open();
                        if (_cells[n - 1, i].CellState != Cell.CellStates.Mine && !_cells[n - 1, i].isOpened) _cells[n - 1, i].Open();
                        if (_cells[n, i - 1].CellState != Cell.CellStates.Mine && !_cells[n, i - 1].isOpened) _cells[n, i - 1].Open();
                    }
                    else if (n == _columns - 1 && i > 0 && i < _rows - 1)
                    {
                        if (_cells[n - 1, i + 1].CellState != Cell.CellStates.Mine && !_cells[n - 1, i + 1].isOpened) _cells[n - 1, i + 1].Open();
                        if (_cells[n - 1, i - 1].CellState != Cell.CellStates.Mine && !_cells[n - 1, i - 1].isOpened) _cells[n - 1, i - 1].Open();
                        if (_cells[n - 1, i].CellState != Cell.CellStates.Mine && !_cells[n - 1, i].isOpened) _cells[n - 1, i].Open();
                        if (_cells[n, i + 1].CellState != Cell.CellStates.Mine && !_cells[n, i + 1].isOpened) _cells[n, i + 1].Open();
                        if (_cells[n, i - 1].CellState != Cell.CellStates.Mine && !_cells[n, i - 1].isOpened) _cells[n, i - 1].Open();
                    }
                    else if (n > 0 && n < _columns - 1 && i > 0 && i < _rows - 1)
                    {
                        if (_cells[n + 1, i].CellState != Cell.CellStates.Mine && !_cells[n + 1, i].isOpened) _cells[n + 1, i].Open();
                        if (_cells[n - 1, i].CellState != Cell.CellStates.Mine && !_cells[n - 1, i].isOpened) _cells[n - 1, i].Open();
                        if (_cells[n, i - 1].CellState != Cell.CellStates.Mine && !_cells[n, i - 1].isOpened) _cells[n, i - 1].Open();
                        if (_cells[n + 1, i - 1].CellState != Cell.CellStates.Mine && !_cells[n + 1, i - 1].isOpened) _cells[n + 1, i - 1].Open();
                        if (_cells[n - 1, i - 1].CellState != Cell.CellStates.Mine && !_cells[n - 1, i - 1].isOpened) _cells[n - 1, i - 1].Open();
                        if (_cells[n + 1, i + 1].CellState != Cell.CellStates.Mine && !_cells[n + 1, i + 1].isOpened) _cells[n + 1, i + 1].Open();
                        if (_cells[n, i + 1].CellState != Cell.CellStates.Mine && !_cells[n, i + 1].isOpened) _cells[n, i + 1].Open();
                        if (_cells[n - 1, i + 1].CellState != Cell.CellStates.Mine && !_cells[n - 1, i + 1].isOpened) _cells[n - 1, i + 1].Open();
                    }
                    else if (n > 0 && n < _columns - 1 && i == 0)
                    {
                        if (_cells[n + 1, i].CellState != Cell.CellStates.Mine && !_cells[n + 1, i].isOpened) _cells[n + 1, i].Open();
                        if (_cells[n - 1, i].CellState != Cell.CellStates.Mine && !_cells[n - 1, i].isOpened) _cells[n - 1, i].Open();
                        if (_cells[n + 1, i + 1].CellState != Cell.CellStates.Mine && !_cells[n + 1, i + 1].isOpened) _cells[n + 1, i + 1].Open();
                        if (_cells[n, i + 1].CellState != Cell.CellStates.Mine && !_cells[n, i + 1].isOpened) _cells[n, i + 1].Open();
                        if (_cells[n - 1, i + 1].CellState != Cell.CellStates.Mine && !_cells[n - 1, i + 1].isOpened) _cells[n - 1, i + 1].Open();
                    }
                    else if (n > 0 && n < _columns - 1 && i == _rows - 1)
                    {
                        if (_cells[n + 1, i].CellState != Cell.CellStates.Mine && !_cells[n + 1, i].isOpened) _cells[n + 1, i].Open();
                        if (_cells[n - 1, i].CellState != Cell.CellStates.Mine && !_cells[n - 1, i].isOpened) _cells[n - 1, i].Open();
                        if (_cells[n + 1, i - 1].CellState != Cell.CellStates.Mine && !_cells[n + 1, i - 1].isOpened) _cells[n + 1, i - 1].Open();
                        if (_cells[n, i - 1].CellState != Cell.CellStates.Mine && !_cells[n, i - 1].isOpened) _cells[n, i - 1].Open();
                        if (_cells[n - 1, i - 1].CellState != Cell.CellStates.Mine && !_cells[n - 1, i - 1].isOpened) _cells[n - 1, i - 1].Open();
                    }
                }
                if (isChecked) return;
            }
        }
    }

    /// <summary>
    /// Sceneを再読み込みする
    /// </summary>
    /// <param name="sceneName"> Sceneの名前 </param>
    public void GameRetry(string sceneName)
    {
        InGame = true;
        SceneManager.LoadScene(sceneName);
    }
}