using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMng : UIBase
{
    private string m_strUIName = string.Empty;
    private static UIMng m_instance = null;
    public static UIMng Instance
    {
        get
        {
            if (m_instance == null)
            {
                UIMng obj = FindObjectOfType<UIMng>();
                m_instance = obj;
            }
            return m_instance;
        }
    }
    private UIMng() { }

    private Dictionary<UIType, UIBase> m_uiDic = new Dictionary<UIType, UIBase>();

    public T GetUI<T>(UIType uiType) where T : UIBase
    {
        if (m_uiDic.ContainsKey(uiType))
        {
            return m_uiDic[uiType] as T;
        }
        return null;
    }

    public T LoadUI<T>(UIType uiType) where T : UIBase
    {
        if (m_uiDic.ContainsKey(uiType))
        {
            return m_uiDic[uiType] as T;
        }

        T uiData = GetComponentInChildren<T>(true);
        if (uiData != null)
        {
            uiData.Init();
            m_uiDic.Add(uiType, uiData);
        }
        return uiData;
    }

    public void DeleteUI(UIType uiType)
    {
        if (m_uiDic.ContainsKey(uiType))
        {
            if (m_uiDic[uiType] != null)
            {
                m_uiDic.Remove(uiType);
            }
        }
    }

    public override void Init()
    {
        DontDestroyOnLoad(this);
    }
}
