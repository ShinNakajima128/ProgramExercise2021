using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MineSweeper : MonoBehaviour
{
    [SerializeField] static int _columns = 10;
    [SerializeField] static int _rows = 10;
    [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
    [SerializeField] Cell _cellPrefab = null;
    [SerializeField] GameObject _gameoverPanel = null;
    [SerializeField] GameObject _gameclearPanel = null;

    [SerializeField] int _mineCount = 1;
    public static int aroundMine = 0;
    public static Cell[,] _cells;
    public static bool InGame = true;
    public static int _closeCellCount;
    public static int _mineCountS;

    void Start()
    {
        InGame = true;
        _mineCountS = _mineCount;
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
        if (_closeCellCount == _mineCount) InGame = false;

        if (!InGame)
        {
            if (_closeCellCount == _mineCount)
            {
                AllCellOpen();
                _gameclearPanel.SetActive(true);
            }
            else
            {
                AllCellOpen();
                _gameoverPanel.SetActive(true);
            }
            
        }
    }
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
    public  static void CheckCells(Cell a)
    {
        bool isChecked = false;

        for (int i = 0; i < _rows; i++)
        {
            for (int n = 0; n < _columns; n++)
            {
                
                if (a.m_indexNum == _cells[n, i].m_indexNum && a.CellState == Cell.CellStates.None && a.isOpened)
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

    public void GameRetry(string sceneName)
    {
        InGame = true;
        SceneManager.LoadScene(sceneName);
    }
}