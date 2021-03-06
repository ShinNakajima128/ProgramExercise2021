using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BingoSystem : MonoBehaviour
{
    [SerializeField]  int m_columns = 5;
    [SerializeField]  int m_rows = 5;
    [SerializeField] GridLayoutGroup m_gridLayoutGroup = null;
    [SerializeField] BingoCell m_cellPrefab = null;
    [SerializeField] Text m_lotNumText = null;
    [SerializeField] GameObject m_bingoText = null;
    [SerializeField] Text m_lotTimesText = null;
    [SerializeField] GameObject m_finishedPanel = null;
    [SerializeField] GameObject[] m_chainLines = null;
    public BingoCell[,] bingocells;
    const int index = 25;
    int lottedNumber = 70;
    int[] bingoCellElements = new int[index];
    int lotNum = 0;
    int[] lottedNums;
    int lottedIndex = 0;
    int rightChain = 0;
    int lowerRightChain = 0;
    int lowerChain = 0;
    int lowerLeftChain = 0;
    bool isFiveChained = false;
    int lotTimes = 0;
    SoundManager soundManager;

    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        soundManager.UnMuteBgm();
        m_bingoText.SetActive(false);
        m_finishedPanel.SetActive(false);
        var parent = m_gridLayoutGroup.transform;
        bingocells = new BingoCell[m_columns, m_rows];
        lottedNums = new int[lottedNumber];

        for (int n = 0; n < index; n++)
        {
            ElementInsert(n);
        }

        for (int n = 1; n <= lottedNumber; n++)
        {
            lottedNums[n - 1] = n;
        }

        for (int n = 0; n < lottedNums.Length; n++)
        {
            int random = Random.Range(0, lottedNumber);
            int temp = lottedNums[n];
            lottedNums[n] = lottedNums[random];
            lottedNums[random] = temp;
        }

        if (m_columns < m_rows)
        {
            m_gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            m_gridLayoutGroup.constraintCount = m_columns;
        }
        else
        {
            m_gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            m_gridLayoutGroup.constraintCount = m_rows;
        }

        for (int i = 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                var cell = Instantiate(m_cellPrefab);
                cell.transform.SetParent(parent);
                bingocells[n, i] = cell;
                if (i == 2 && n == 2)
                {
                    bingocells[n, i].isCellOpened = true;
                    bingocells[n, i].BG.color = new Color(0.93f, 0.58f, 0.25f);
                    continue;
                }
                else
                {
                    bingocells[n, i].BG.color = new Color(0.44f, 0.44f, 0.44f);
                }
                cell.CellNum = bingoCellElements[i * m_rows + n];
            }
        }
    }

    void Update()
    {
        if (m_lotNumText != null) m_lotNumText.text = lotNum.ToString();
        if (m_lotTimesText != null) m_lotTimesText.text = "抽選回数：" + lotTimes.ToString() + "回";

        if (isFiveChained)
        {
            Debug.Log("BINGO!");
            soundManager.StopBgm();
            soundManager.PlaySeByName("Bingo");
            m_bingoText.SetActive(true);
            m_finishedPanel.SetActive(true);
            isFiveChained = false;
        }
    }

    public void ElementInsert(int x)
    {
        bingoCellElements[x] = Random.Range(1, 71);

        if (ElementCheck(x,bingoCellElements))
        {
            return;
        }
        else
        {
            ElementInsert(x);
        }
    }

    public bool ElementCheck(int x, int[] CheckArray)
    {
        for (int i = x - 1; i >= 0; i--)
        {
            if (CheckArray[i] == CheckArray[x])
            {
                return false;
            }
            else
            {
                continue;
            }
        }
        return true;
    }

    public void LotteryNum()
    {
        soundManager.PlaySeByName("Lot");
        lotNum = lottedNums[lottedIndex];
        SameNumberCheck(lotNum);
        lottedIndex++;
        lotTimes++;
        Bingocheck();
    }

    public void SameNumberCheck(int lotnum)
    {
        for (int i = 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                if (bingocells[n, i].CellNum == lotnum)
                {
                    bingocells[n, i].isCellOpened = true;
                    bingocells[n, i].BG.color = new Color(0.93f, 0.58f, 0.25f);
                }
            }
        }
    }

    public void Bingocheck()
    {
        for (int i = 0; i < m_rows; i++)
        {
            for (int n = 0; n < m_columns; n++)
            {
                if (i != 0 && n > 0) break;

                if (i == 0)
                {
                    if (n == 0)
                    {
                        if (bingocells[n, i].isCellOpened)
                        {
                            rightChain = 1;
                            lowerRightChain = 1;
                            lowerChain = 1;
                            RightCheck(n, i, i);
                            LowerRightCheck(n, i);
                            LowerCheck(n, i, n);
                        }
                    }
                    else if (n < m_columns - 1)
                    {
                        if (bingocells[n, i].isCellOpened)
                        {
                            lowerChain = 1;
                            LowerCheck(n, i, n);
                        }
                    }
                    else if (n == m_columns - 1)
                    {
                        if (bingocells[n, i].isCellOpened)
                        {
                            lowerChain = 1;
                            lowerLeftChain = 1;
                            LowerCheck(n, i, n);
                            LowerLeftCheck(n, i);
                        }   
                    }    
                }
                else if (i < m_rows)
                {
                    if (bingocells[n, i].isCellOpened)
                    {
                        rightChain = 1;
                        RightCheck(n, i, i);
                    }
                }
            }
        }
    }

    public void RightCheck(int x, int y, int lineNum)
    {
        int right = x + 1;

        if (right < m_rows)
        {
            if (bingocells[right, y].isCellOpened)
            {
                rightChain++;
                RightCheck(right, y,lineNum);
            }
        }
        else
        {
            if (rightChain == 5) isFiveChained = true;
            if (lineNum == 0) m_chainLines[0].SetActive(true);
            if (lineNum == 1) m_chainLines[1].SetActive(true);
            if (lineNum == 2) m_chainLines[2].SetActive(true);
            if (lineNum == 3) m_chainLines[3].SetActive(true);
            if (lineNum == 4) m_chainLines[4].SetActive(true);
        }
    }

    public void LowerRightCheck(int x, int y)
    {
        int right = x + 1;
        int bottom = y + 1;

        if (right < m_rows && bottom < m_columns)
        {
            if (bingocells[right, bottom].isCellOpened)
            {
                lowerRightChain++;
                LowerRightCheck(right, bottom);
            }
        }
        else
        {
            if (lowerRightChain == 5) isFiveChained = true;
            m_chainLines[11].SetActive(true);
        }
    }

    public void LowerCheck(int x, int y, int lineNum)
    {
        int bottom = y + 1;

        if (bottom < m_columns)
        {
            if (bingocells[x, bottom].isCellOpened)
            {
                lowerChain++;
                LowerCheck(x, bottom, lineNum);
            }
        }
        else
        {
            if (lowerChain == 5) isFiveChained = true;
            if (lineNum == 0) m_chainLines[5].SetActive(true);
            if (lineNum == 1) m_chainLines[6].SetActive(true);
            if (lineNum == 2) m_chainLines[7].SetActive(true);
            if (lineNum == 3) m_chainLines[8].SetActive(true);
            if (lineNum == 4) m_chainLines[9].SetActive(true);
        }
    }

    public void LowerLeftCheck(int x, int y)
    {
        int left = x - 1;
        int bottom = y + 1;

        if (left >= 0 && bottom < m_columns)
        {
            if (bingocells[left, bottom].isCellOpened)
            {
                lowerLeftChain++;
                LowerLeftCheck(left, bottom);
            }
        }
        else
        {
            if (lowerLeftChain == 5) isFiveChained = true;
            m_chainLines[10].SetActive(true);
        }
    }

    public void GameRetry(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
