using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScript2 : MonoBehaviour
{
    [SerializeField] int m_Index_X = 5;
    [SerializeField] int m_Index_Y = 5;

    GameObject[,] cubes;

    int m_selectIndex_X = 0;
    int m_selectIndex_Y = 0;

    void Start()
    {
        cubes = new GameObject[m_Index_X, m_Index_Y];

        for (int i = 0; i < m_Index_Y; i++)
        {
            for (int n = 0; n < m_Index_X; n++)
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubes[n, i] = cube;
                cube.transform.position = new Vector3(-4f + (n * 2), -4f + (i * 2), 0);
                var r = cube.GetComponent<Renderer>();
                r.material.color = (n == 0 && i == 0) ? Color.red : Color.white;
            }
        }
    }

    void Update()
    {
        SampleExercise1();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            cubes[m_selectIndex_X, m_selectIndex_Y].SetActive(false);
        }
    }

    void SampleExercise1()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (m_selectIndex_Y == m_Index_Y - 1) return;
            while (!cubes[m_selectIndex_X, m_selectIndex_Y + 1].activeSelf)
            {
                m_selectIndex_Y++;
            }
            m_selectIndex_Y++;
            UpdateCubes();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (m_selectIndex_Y == 0) return;
            while (!cubes[m_selectIndex_X, m_selectIndex_Y - 1].activeSelf)
            {
                m_selectIndex_Y--;
            }
            m_selectIndex_Y--;
            UpdateCubes();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (m_selectIndex_X == m_Index_X - 1) return;
            while (!cubes[m_selectIndex_X + 1, m_selectIndex_Y].activeSelf)
            {
                m_selectIndex_X++;
            }
            m_selectIndex_X++;
            UpdateCubes();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (m_selectIndex_X == 0) return;
            while (!cubes[m_selectIndex_X - 1, m_selectIndex_Y].activeSelf)
            {
                m_selectIndex_X--;
            }
            m_selectIndex_X--;
            UpdateCubes();
        }
    }

    private void UpdateCubes()
    {
        for (int i = 0; i < m_Index_Y; i++)
        {
            for (int n = 0; n < m_Index_X; n++)
            {
                var r = cubes[n, i].GetComponent<Renderer>();
                r.material.color = (n == m_selectIndex_X && i == m_selectIndex_Y) ? Color.red : Color.white;
            }
        }
    }
}
