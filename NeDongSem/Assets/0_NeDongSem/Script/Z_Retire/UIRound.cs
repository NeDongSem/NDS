using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIRound : UIBase
{
    private TextMeshProUGUI m_roundText;
    private string m_strRound = "Round";
    private int m_roundNumber = 1; public int RoundNumber { get { return m_roundNumber; } set { m_roundNumber = value; } }

    public override void Init()
    {
        SetRoundText(m_roundNumber);
    }

    public void SetRoundText(int _roundNumber)
    {
        m_roundNumber = _roundNumber;
        m_strRound += " "+_roundNumber.ToString();
        m_roundText = GetComponent<TextMeshProUGUI>();
        m_roundText.text = m_strRound;
    }
}
