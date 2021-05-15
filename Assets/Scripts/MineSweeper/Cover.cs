using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cover : MonoBehaviour
{
    [SerializeField] CoverStates m_coverState = CoverStates.Close;
    [SerializeField]Image m_coverImage = null;

    public CoverStates CoverState
    {
        get => m_coverState;
        set
        {
            m_coverState = value;
            OnCoverStateChanged();
        }
    }
    public enum CoverStates
    {
        Close,
        Select,
        Open
    }
    void OnValidate()
    {
        OnCoverStateChanged();
    }

    void Update()
    {
        OnCoverStateChanged();
    }


    void OnCoverStateChanged()
    {
        if (m_coverState == CoverStates.Close)
        {
            if (!m_coverImage.enabled) m_coverImage.enabled = true;
            m_coverImage.color = new Color(0, 1f, 1f);
        }
        else if (m_coverState == CoverStates.Select)
        {
            if (!m_coverImage.enabled) m_coverImage.enabled = true;
            m_coverImage.color = new Color(1f, 0, 0);
        }
        else
        {
            m_coverImage.enabled = false;
        }
    }

    public void Open()
    {
        m_coverState = CoverStates.Open;
    }
}
