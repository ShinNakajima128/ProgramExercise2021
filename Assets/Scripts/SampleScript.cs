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
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (m_selectIndex == cubes.Length - 1)
            {
                //return;
    
                cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.white;
                cubes[0].GetComponent<Renderer>().material.color = Color.red;
                m_selectIndex = 0;

                Debug.Log(m_selectIndex);

                return;
            }

            
            cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.white;
            m_selectIndex++;
            cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.red;
            
            Debug.Log(m_selectIndex);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (m_selectIndex == 0)
            {
                //return;
                
                cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.white;
                cubes[cubes.Length - 1].GetComponent<Renderer>().material.color = Color.red;

                m_selectIndex = cubes.Length - 1;

                Debug.Log(m_selectIndex);

                return;
            }

            cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.white;
            m_selectIndex--;
            cubes[m_selectIndex].GetComponent<Renderer>().material.color = Color.red;

            Debug.Log(m_selectIndex);
        }
    }
}
