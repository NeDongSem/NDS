using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Hp : MonoBehaviour
{
    public int m_iHp = 3;
    List<GameObject> m_HpObjList;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        SettingHpList();
    }

    private void SettingHpList()
    {
        m_HpObjList = new List<GameObject>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            m_HpObjList.Add(transform.GetChild(i).gameObject);
        }

        for (int i = m_iHp; i < transform.childCount; ++i)
        {
            m_HpObjList[i].SetActive(false);
        }
    }

    public bool Set_EnemyGoal()
    {
        m_iHp -= 1;

        for (int i = m_iHp; i < transform.childCount; ++i)
        {
            m_HpObjList[i].SetActive(false);
        }

        if (m_iHp == 0)
        {
            return false;
        }

        return true;
    }
}
