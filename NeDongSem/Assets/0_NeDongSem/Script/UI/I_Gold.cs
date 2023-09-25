using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class I_Gold : MonoBehaviour
{
    public TextMeshProUGUI m_GoldTxt;
    public int m_iGold = 70;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        Set_Gold(0);
    }

    public bool Set_Gold(int _iAddGold)
    {
        if(m_iGold + _iAddGold < 0)
        {
            return false;
        }

        m_iGold += _iAddGold;
        m_GoldTxt.text = m_iGold.ToString() + 'G';
        return true;
    }
}
