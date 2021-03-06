using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject m_mainMenu = null;
    [SerializeField] GameObject m_optionMenu = null;
    [SerializeField] GameObject m_AudioMenu = null;
    [SerializeField] GameObject m_gameBar = null;
    [SerializeField] Dropdown m_sceneDropdowm = null;
    [SerializeField] GameObject m_guideText = null;
    SoundManager soundManager;
    TitleState m_titleState = TitleState.None;
    bool isUpdated = false;

    enum TitleState
    {
        None,
        Main,
        Option,
        Audio
    }

    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        soundManager.UnMuteBgm();
        m_gameBar.SetActive(false);
        m_mainMenu.SetActive(false);
        m_optionMenu.SetActive(false);
        m_AudioMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.anyKeyDown && m_guideText.activeSelf)
        {
            m_titleState = TitleState.Main;
            soundManager.PlaySeByName("Select");
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !m_guideText.activeSelf)
        {
            m_titleState = TitleState.None;
            soundManager.PlaySeByName("Select");
        }

        switch (m_titleState)
        {
            case TitleState.None:
                m_guideText.SetActive(true);
                m_gameBar.SetActive(false);
                m_mainMenu.SetActive(false);
                m_optionMenu.SetActive(false);
                m_AudioMenu.SetActive(false);
                break;
            case TitleState.Main:
                m_guideText.SetActive(false);
                m_gameBar.SetActive(true);
                m_mainMenu.SetActive(true);
                m_optionMenu.SetActive(false);
                m_AudioMenu.SetActive(false);
                break;
            case TitleState.Option:
                m_gameBar.SetActive(false);
                m_mainMenu.SetActive(false);
                m_optionMenu.SetActive(true);
                m_AudioMenu.SetActive(false);
                break;
            case TitleState.Audio:
                m_gameBar.SetActive(false);
                m_mainMenu.SetActive(false);
                m_optionMenu.SetActive(false);
                m_AudioMenu.SetActive(true);

                if (!isUpdated)
                {
                    GameObject.Find("MasterSlider").GetComponent<Slider>().value = SoundManager.m_masterVolume;
                    GameObject.Find("BGMSlider").GetComponent<Slider>().value = SoundManager.m_bgmVolume;
                    GameObject.Find("SESlider").GetComponent<Slider>().value = SoundManager.m_seVolume;
                    isUpdated = true;
                    Debug.Log("check");
                }
                
                break;
        }
    }
    public void LoadGame()
    {
        soundManager.PlaySeByName("GameStart");
        SceneManager.LoadScene(m_sceneDropdowm.captionText.text);
    }

    public void MainSelect()
    {
        m_titleState = TitleState.Main;
        soundManager.PlaySeByName("Select");
    }
    public void OptionSelect()
    {
        m_titleState = TitleState.Option;
        soundManager.PlaySeByName("Select");
    }
    public void AudioSelect()
    {
        m_titleState = TitleState.Audio;
        soundManager.PlaySeByName("Select");
    }

    public void ExitGame()
    {
        soundManager.PlaySeByName("Select");
        Application.Quit();
    }

    public void SelectGameSE()
    {
        soundManager.PlaySeByName("Select");
    }
}
