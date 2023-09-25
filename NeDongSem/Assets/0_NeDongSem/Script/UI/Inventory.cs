using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, GameObject> m_ChildObjDictionary;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        SettingChildObj();
    }

    private void SettingChildObj()
    {
        m_ChildObjDictionary = new Dictionary<string, GameObject>();

        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject ChildObj = transform.GetChild(i).gameObject;
            m_ChildObjDictionary.Add(ChildObj.name, ChildObj);
        }
    }

    public bool Set_EnemyGoal()
    {
        return m_ChildObjDictionary["I_Hp"].GetComponent<I_Hp>().Set_EnemyGoal();
    }

    public bool Set_Gold(int _iAddGold)
    {
        return m_ChildObjDictionary["I_Gold"].GetComponent<I_Gold>().Set_Gold(_iAddGold);
    }
}
