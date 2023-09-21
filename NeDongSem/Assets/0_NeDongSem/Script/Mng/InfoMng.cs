using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoMng : MonoBehaviour
{
    static private InfoMng instance = null;
    static public InfoMng Instance { get { return instance; } }

    public string m_TowerSheetRange = "A1:H25";
    public string m_EnemySheetRange = "A1:C51";
    List<string> m_TowerInfoTitleList;
    Dictionary<string, Dictionary<string,string>> m_TowerInfoDictionary;
    List<string> m_EnemyInfoTitleList;
    Dictionary<string, Dictionary<string, string>> m_EnemyInfoDictionary; public Dictionary<string, Dictionary<string, string>> EnemyInfoDictionary { get { return m_EnemyInfoDictionary; } }

    private void Awake()
    {
        //InstanceCheck
        if (ReferenceEquals(instance, null))
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            StartCoroutine("Init");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private IEnumerator Init()
    {
        while (ReferenceEquals(GoogleSheetMng.Instance, null))
        {
            yield return new WaitForSeconds(0.1f);
        }

        GoogleSheetMng.delDataProcessingFunc delTowerInfoSettingFunc = new GoogleSheetMng.delDataProcessingFunc(TowerInfoSetting);
        GoogleSheetMng.Instance.Get_GoogleSheetData("Tower", m_TowerSheetRange, delTowerInfoSettingFunc);

        GoogleSheetMng.delDataProcessingFunc delEnemyInfoSettingFunc = new GoogleSheetMng.delDataProcessingFunc(EnemyInfoSetting);
        GoogleSheetMng.Instance.Get_GoogleSheetData("Enemy", m_EnemySheetRange, delEnemyInfoSettingFunc);
    }

    private void TowerInfoSetting(string[] _strTowerInfoArray)
    {
        m_TowerInfoTitleList = new List<string>();
        m_TowerInfoDictionary = new Dictionary<string, Dictionary<string, string>>();

        string[] strTowerInfoTitleArray = _strTowerInfoArray[0].Split(",");
        for (int i = 0; i < strTowerInfoTitleArray.Length; ++i)
        {
            m_TowerInfoTitleList.Add(strTowerInfoTitleArray[i]);
        }

        for(int i = 1; i < _strTowerInfoArray.Length; ++i)
        {
            string[] strTowerInfoArray = _strTowerInfoArray[i].Split(",");
            Dictionary<string, string> TowerInfoDictionary = new Dictionary<string, string>();

            for(int j = 0; j < strTowerInfoArray.Length; ++j)
            {
                TowerInfoDictionary.Add(m_TowerInfoTitleList[j], strTowerInfoArray[j]);
            }
            m_TowerInfoDictionary.Add(strTowerInfoArray[0], TowerInfoDictionary);
        }
    }
    private void EnemyInfoSetting(string[] _strEnemyInfoArray)
    {
        m_EnemyInfoTitleList = new List<string>();
        m_EnemyInfoDictionary = new Dictionary<string, Dictionary<string, string>>();

        string[] strEnemyInfoTitleArray = _strEnemyInfoArray[0].Split(",");
        for (int i = 0; i < strEnemyInfoTitleArray.Length; ++i)
        {
            m_EnemyInfoTitleList.Add(strEnemyInfoTitleArray[i]);
        }

        for (int i = 1; i < _strEnemyInfoArray.Length; ++i)
        {
            string[] strEnemyInfoArray = _strEnemyInfoArray[i].Split(",");
            Dictionary<string, string> EnemyInfoDictionary = new Dictionary<string, string>();

            for (int j = 0; j < strEnemyInfoArray.Length; ++j)
            {
                EnemyInfoDictionary.Add(m_EnemyInfoTitleList[j], strEnemyInfoArray[j]);
            }
            m_EnemyInfoDictionary.Add(strEnemyInfoArray[0], EnemyInfoDictionary);
        }
    }

    public string Get_TowerInfo(string _strTowerName, string _strTowerInfoTitle)
    {
        if(m_TowerInfoDictionary == null)
        {
            return "Not Ready Info";
        }

        return m_TowerInfoDictionary[_strTowerName][_strTowerInfoTitle];
    }

    public string Get_EnemyInfo(string _strEnemyName, string _strEnemyInfoTitle)
    {
        if (m_EnemyInfoDictionary == null)
        {
            return "Not Ready Info";
        }

        return m_EnemyInfoDictionary[_strEnemyName][_strEnemyInfoTitle];
    }
}
