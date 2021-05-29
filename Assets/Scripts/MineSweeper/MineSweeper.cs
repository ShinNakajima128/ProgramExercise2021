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
    int aroundMine = 0;
    public static Cell[,] _cells;
    public static bool InGame = true;
    public static int _closeCellCount;

    void Start()
    {
        
        _closeCellCount = _columns * _rows;
        _gameoverPanel.SetActive(false);
        _gameclearPanel.SetActive(false);

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

        for (var i = 0; i < _mineCount;)
        {
            var r = Random.Range(0, _rows);
            var c = Random.Range(0, _columns);
            var cell = _cells[r, c];

            if (cell.CellState == Cell.CellStates.Mine)
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
    private void Update()
    {

        if (!InGame)
        {
            _gameoverPanel.SetActive(true);
        }

        if (_closeCellCount == _mineCount)
        {
            _gameclearPanel.SetActive(true);
        }
    }

    void StartCell()
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
                    Debug.Log(n);
                    Debug.Log(i);
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