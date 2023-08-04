using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIGold : UIBase
{
    private TextMeshProUGUI m_goldText;
    private string m_strGold = "Gold";
    private int m_goldNumber = 0; public int RoundNumber { get { return m_goldNumber; } set { m_goldNumber = value; } }

    public override void Init()
    {
        SetRoundText(m_goldNumber);
    }

    public void SetRoundText(int _roundNumber)
    {
        m_goldNumber = _roundNumber;
        m_strGold += " " + _roundNumber.ToString();
        m_goldText = GetComponent<TextMeshProUGUI>();
        m_goldText.text = m_strGold;
    }
}
