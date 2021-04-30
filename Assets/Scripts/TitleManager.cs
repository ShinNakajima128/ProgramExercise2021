using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Dropdown m_sceneDropdowm = null;

    public void LoadGame()
    {
        SceneManager.LoadScene(m_sceneDropdowm.captionText.text);
    }
}
