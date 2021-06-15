using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    public static float m_masterVolume = 1.0f;
    public static float m_bgmVolume = 0.5f;
    public static float m_seVolume = 1.0f;
    [SerializeField] AudioClip[] m_bgms = null;
    [SerializeField] AudioClip[] m_ses = null;
    [SerializeField] AudioSource m_bgmAudioSource = null;
    [SerializeField] AudioSource m_seAudioSource = null;
    Dictionary<string, int> bgmIndex = new Dictionary<string, int>();
    Dictionary<string, int> seIndex = new Dictionary<string, int>();
    public static bool isLosted = false;
   

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < m_bgms.Length; i++)
        {
            bgmIndex.Add(m_bgms[i].name, i);
        }

        for (int i = 0; i < m_ses.Length; i++)
        {
            seIndex.Add(m_ses[i].name, i);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SceneManager.GetActiveScene().name == "Title")
        {
            PlayBgmByName("Title");
        }
        else if (SceneManager.GetActiveScene().name == "MineSweeper")
        {
            PlayBgmByName("MineSweeper");
        }
        else if (SceneManager.GetActiveScene().name == "LifeGame")
        {
            PlayBgmByName("LifeGame");
        }
        else if (SceneManager.GetActiveScene().name == "Bingo")
        {
            PlayBgmByName("Bingo");
        }
        else if (SceneManager.GetActiveScene().name == "Reversi")
        {
            PlayBgmByName("Reversi");
        }
    }

    /// <summary>
    /// Sceneが遷移した時にBGMを変更する
    /// </summary>
    /// <param name="nextScene">遷移後のScene</param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Title":
                PlayBgmByName("Title");
                break;
            case "MineSweeper":
                PlayBgmByName("MineSweeper");
                break;
            case "LifeGame":
                PlayBgmByName("LifeGame");
                break;
            case "Bingo":
                PlayBgmByName("Bingo");
                break;
            case "Reversi":
                PlayBgmByName("Reversi");
                break;
        }
    }

    void Update()
    {
        m_bgmAudioSource.volume = m_bgmVolume * m_masterVolume;
        m_seAudioSource.volume = m_seVolume * m_masterVolume;


        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayBgmByName("Title");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StopBgm();
        }
    }

    public int GetBgmIndex(string name)
    {
        if (bgmIndex.ContainsKey(name))
        {
            return bgmIndex[name];
        }
        else
        {
            return 0;
        }
    }

    public int GetSeIndex(string name)
    {
        if (seIndex.ContainsKey(name))
        {
            return seIndex[name];
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="index"> BGMの添え字番号 </param>
    public void PlayBgm(int index)
    {
        index = Mathf.Clamp(index, 0, m_bgms.Length);

        m_bgmAudioSource.clip = m_bgms[index];
        m_bgmAudioSource.loop = true;
        m_bgmAudioSource.volume = m_bgmVolume * m_masterVolume;
        m_bgmAudioSource.Play();
    }

    /// <summary>
    /// BGM名を入力し、BGMを再生する
    /// </summary>
    /// <param name="name"> BGMの名前 </param>
    public void PlayBgmByName(string name)
    {
        PlayBgm(GetBgmIndex(name));
    }

    /// <summary>
    /// BGMを止める
    /// </summary>
    public void StopBgm()
    {
        m_bgmAudioSource.Stop();
        m_bgmAudioSource.clip = null;
    }

    /// <summary>
    /// BGMのミュートを解除する
    /// </summary>
    public void UnMuteBgm()
    {
        if (m_bgmAudioSource.mute) m_bgmAudioSource.mute = false;
    }

    /// <summary>
    /// BGMをミュートする
    /// </summary>
    public void MuteBgm()
    {
        m_bgmAudioSource.mute = true;
    }

    public void PlaySe(int index)
    {
        index = Mathf.Clamp(index, 0, m_ses.Length);

        m_seAudioSource.PlayOneShot(m_ses[index], m_seVolume * m_masterVolume);
    }

    public void PlaySeByName(string name)
    {
        PlaySe(GetSeIndex(name));
    }

    public void StopSe()
    {
        m_seAudioSource.Stop();
        m_seAudioSource.clip = null;
    }

    public void MasterVolChange()
    {
        m_masterVolume = GameObject.Find("MasterSlider").GetComponent<Slider>().value;
        Debug.Log(m_masterVolume);
    }
    public void BGMVolChange()
    {
        m_bgmVolume = GameObject.Find("BGMSlider").GetComponent<Slider>().value;
        Debug.Log(m_bgmVolume);
    }
    public void SEVolChange()
    {
        m_seVolume = GameObject.Find("SESlider").GetComponent<Slider>().value;
        PlaySeByName("Select");
        Debug.Log(m_seVolume);
    }
}