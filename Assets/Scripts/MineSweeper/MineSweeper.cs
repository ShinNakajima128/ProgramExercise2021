using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineSweeper : MonoBehaviour
{
    [SerializeField] static int _columns = 10;
    [SerializeField] static int _rows = 10;
    [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
    [SerializeField] Cell _cellPrefab = null;
    [SerializeField] Text _gameoverText = null;

    [SerializeField] int _mineCount = 1;
    int aroundMine = 0;
    public static Cell[,] _cells;
    public static bool InGame = true;

    void Start()
    {
        _gameoverText.enabled = false;

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

        for (var i = 0; i < _mineCount; i++)
        {
            var r = Random.Range(0, _rows);
            var c = Random.Range(0, _columns);
            var cell = _cells[r, c];

            if (cell.CellState != Cell.CellStates.Mine)
            {
                cell.CellState = Cell.CellStates.Mine;
            }
            else
            {
                _mineCount++;
            }
        }

        StartCell();

    }
    private void Update()
    {
        
        if (!InGame)
        {
            _gameoverText.enabled = true;
            _gameoverText.text = "GAMEOVER";
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
                    else if (n == 0 && i == _rows)
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
                    else if (n == 0 && i == _rows)
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
                }

                if (isChecked) return;
            }
        }
    }
}