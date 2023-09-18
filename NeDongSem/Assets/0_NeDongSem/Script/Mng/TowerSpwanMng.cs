using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum eTowerType
{
    Nemo,
    Dongrami,
    Semo,
    eTowerType_End
}

public enum eTowerLevel
{
    Lv1 = 0,
    Lv2,
    Lv3,
    Lv4,
    Lv5,
    Power,
    Cri,
    Stun,
    Range,
    Aoe,
    Speed,
    Slow,
    eTowerLevel_End
}

public class TowerSpwanMng : MonoBehaviour
{
    static private TowerSpwanMng instance = null;
    static public TowerSpwanMng Instance { get { return instance; } }

    public GameObject m_NemoPrefab;
    public GameObject m_DongramiPrefab;
    public GameObject m_SemoPrefab;

    public List<Sprite> m_NemoSpriteList;
    public List<Sprite> m_DongramiSpriteList;
    public List<Sprite> m_SemoSpriteList;

    stTowerInfo m_stTowerInfo;

    float m_fTowerZ = -0.05f;
    float m_fTowerAddZ = -0.001f;
    int m_iSpwanNum = 0;

    private void Awake()
    {
        //InstanceCheck
        if (ReferenceEquals(instance, null))
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Init()
    {
        m_stTowerInfo = new stTowerInfo();
    }

    public void Set_Reset() //스테이지 씬 새로 들어갈 때 마다 불러줘야 함
    {
        m_iSpwanNum = 0;
    }

    public GameObject Set_TowerSpwan(Vector3 _v3SpawnPos, eTowerType _eTowerType)
    {
        TowerInfoSetting(_eTowerType);
        GameObject SpwanTowerGameObject = null;
        switch (_eTowerType)
        {
            case eTowerType.Nemo:
                SpwanTowerGameObject = Instantiate(m_NemoPrefab);
                SpwanTowerGameObject.transform.position = _v3SpawnPos;
                break;
            case eTowerType.Dongrami:
                SpwanTowerGameObject = Instantiate(m_DongramiPrefab);
                SpwanTowerGameObject.transform.position = _v3SpawnPos;
                break;
            case eTowerType.Semo:
                SpwanTowerGameObject = Instantiate(m_SemoPrefab);
                SpwanTowerGameObject.transform.position = _v3SpawnPos;
                break;
            case eTowerType.eTowerType_End:
                break;
        }

        SpwanTowerGameObject.GetComponent<Tower>().TowerInfo = m_stTowerInfo;
        Vector3 TowerPos = SpwanTowerGameObject.transform.position;
        TowerPos.z = m_fTowerZ + (m_fTowerAddZ * ++m_iSpwanNum);
        SpwanTowerGameObject.transform.position = TowerPos;

        return SpwanTowerGameObject;
    }

    private void TowerInfoSetting(eTowerType _eTowerType)
    {
        switch (_eTowerType)
        {
            case eTowerType.Nemo:
                m_stTowerInfo.strName = "Nemo";
                m_stTowerInfo.eTowerType = eTowerType.Nemo;
                break;
            case eTowerType.Dongrami:
                m_stTowerInfo.strName = "Dongrami";
                m_stTowerInfo.eTowerType = eTowerType.Dongrami;
                break;
            case eTowerType.Semo:
                m_stTowerInfo.strName = "Semo";
                m_stTowerInfo.eTowerType = eTowerType.Semo;
                break;
            case eTowerType.eTowerType_End:
                break;
        }
        m_stTowerInfo.strLevel = "Lv1";
        m_stTowerInfo.eTowerLevel = eTowerLevel.Lv1;
        string strTowerFullName = m_stTowerInfo.strName + "_" + m_stTowerInfo.strLevel;
        m_stTowerInfo.fDis = float.Parse(InfoMng.Instance.Get_TowerInfo(strTowerFullName, "Dis"));
        m_stTowerInfo.fAps = float.Parse(InfoMng.Instance.Get_TowerInfo(strTowerFullName, "Aps"));
        m_stTowerInfo.fAtk = float.Parse(InfoMng.Instance.Get_TowerInfo(strTowerFullName, "Atk"));
        m_stTowerInfo.fCri = float.Parse(InfoMng.Instance.Get_TowerInfo(strTowerFullName, "Cri"));
        m_stTowerInfo.eCC = (eCCType)System.Enum.Parse(typeof(eCCType), InfoMng.Instance.Get_TowerInfo(strTowerFullName, "CC"));
        m_stTowerInfo.fCCValue1 = float.Parse(InfoMng.Instance.Get_TowerInfo(strTowerFullName, "CCValue1"));
        m_stTowerInfo.fCCValue2 = float.Parse(InfoMng.Instance.Get_TowerInfo(strTowerFullName, "CCValue2"));
    }

    public Sprite Get_TowerSprite(eTowerType _eTowerType, eTowerLevel _eTowerLevel)
    {
        int iLevel = ((int)_eTowerLevel);
        if(iLevel >= 10)
        {
            iLevel -= 4;
        }
        else if (iLevel >= 8)
        {
            iLevel -= 2;
        }

        switch (_eTowerType)
        {
            case eTowerType.Nemo:
                return m_NemoSpriteList[iLevel];
            case eTowerType.Dongrami:
                return m_DongramiSpriteList[iLevel];
            case eTowerType.Semo:
                return m_SemoSpriteList[iLevel];
            case eTowerType.eTowerType_End:
                break;
        }

        return null;
    }
}
