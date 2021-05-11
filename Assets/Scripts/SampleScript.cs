using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScript : MonoBehaviour
{
    [SerializeField] int m_indexNum = 8;
    GameObject[] cubes;
    int m_selectIndex = 0;

    void Start()
    {
        cubes = new GameObject[m_indexNum];

        for (int i = 0; i < m_indexNum; i++)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubes[i] = cube;
            cube.transform.position = new Vector3(-4f + (i * 2), 0, 0);
            var r = cube.GetComponent<Renderer>();
            r.material.color = (i == 0) ? Color.red : Color.white;
        }
    }

    void Update()
    {
        SelectCube();
        EraseCube();
    }

    public void SelectCube()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (m_selectIndex == cubes.Length - 1 || !RightCubeIndexSearch(m_selectIndex))
            {
                cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.white;

                m_selectIndex = 0;


                while (!cubes[m_selectIndex].activeSelf)
                {
                    m_selectIndex++;
                }

                cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.red;

                Debug.Log(m_selectIndex);
            }
            else
            {
                RightCubeIndexSearch(m_selectIndex);

                cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.white;

                while (!cubes[m_selectIndex + 1].activeSelf)
                {
                    m_selectIndex++;
                }
                m_selectIndex++;
                cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.red;

                Debug.Log(m_selectIndex);
            }           
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (m_selectIndex == 0 || !LeftCubeIndexSearch(m_selectIndex))
            {

                cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.white;
                m_selectIndex = cubes.Length - 1;

                while (!cubes[m_selectIndex].activeSelf)
                {
                    m_selectIndex--;
                }

                cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.red;

                Debug.Log(m_selectIndex);
            }
            else
            {
                LeftCubeIndexSearch(m_selectIndex);

                cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.white;

                while (!cubes[m_selectIndex - 1].activeSelf)
                {
                    m_selectIndex--;
                }
                m_selectIndex--;
                cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.red;

                Debug.Log(m_selectIndex);
            }
        }
    }

    public void EraseCube()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            cubes[m_selectIndex].SetActive(false);

            if (m_selectIndex < cubes.Length - 1 && cubes[m_selectIndex + 1].activeSelf)
            {
                while (!cubes[m_selectIndex].activeSelf)
                {
                    m_selectIndex++;
                }

                cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.red;
                Debug.Log(m_selectIndex);
            }
            else if (m_selectIndex > 0 && cubes[m_selectIndex - 1].activeSelf)
            {
                while (!cubes[m_selectIndex].activeSelf)
                {
                    m_selectIndex--;
                }

                cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.red;
                Debug.Log(m_selectIndex);
            }
            else
            {
                if (!RightCubeIndexSearch(m_selectIndex))
                {
                    if (cubes[m_selectIndex].activeSelf)
                    {
                        while (!cubes[m_selectIndex - 1].activeSelf)
                        {
                            m_selectIndex--;
                        }
                        m_selectIndex--;
                        cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.red;
                    }
                    else
                    {
                        return;
                    }
                }
                else if (!LeftCubeIndexSearch(m_selectIndex))
                {
                    while (!cubes[m_selectIndex + 1].activeSelf)
                    {
                        m_selectIndex++;
                    }
                    m_selectIndex++;
                    cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }

    void Example()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_selectIndex--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_selectIndex++;
        }

        
        for (int i = 0; i < cubes.Length; i++)
        {
            var renderer = cubes[i].GetComponent<Renderer>();
            renderer.material.color = (i == m_selectIndex ? Color.red : Color.white);
        }
    }
    bool RightCubeIndexSearch(int currentIndex)
    {
        for (int i = currentIndex + 1; i < cubes.Length; i++)
        {
            if (cubes[i].activeSelf)
            {
                Debug.Log("Cubeがありました");

                return true;
            }
        }
        Debug.Log("Cubeが見つかりませんでした");

        return false; 
    }

    bool LeftCubeIndexSearch(int currentIndex)
    {
        for (int i = currentIndex - 1; i >= 0; i--)
        {
            if (cubes[i].activeSelf)
            {
                Debug.Log("Cubeがありました");
                return true;
            }
        }
        Debug.Log("Cubeが見つかりませんでした");
        return false;
    }
}
