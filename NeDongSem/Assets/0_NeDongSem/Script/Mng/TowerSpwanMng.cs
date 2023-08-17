using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eTowerType
{
    Nemo,
    Dongrami,
    Semo,
    eTowerType_End
}

public class TowerSpwanMng : MonoBehaviour
{
    static private TowerSpwanMng instance = null;
    static public TowerSpwanMng Instance { get { return instance; } }

    public GameObject m_NemoPrefab;
    public GameObject m_DongramiPrefab;
    public GameObject m_SemoPrefab;

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
                break;
            case eTowerType.Dongrami:
                m_stTowerInfo.strName = "Dongrami";
                break;
            case eTowerType.Semo:
                m_stTowerInfo.strName = "Semo";
                break;
            case eTowerType.eTowerType_End:
                break;
        }
        m_stTowerInfo.strLevel = "Lv1";
        string strTowerFullName = m_stTowerInfo.strName + "_" + m_stTowerInfo.strLevel;
        m_stTowerInfo.fAPS = float.Parse(InfoMng.Instance.Get_TowerInfo(strTowerFullName, "Aps"));
        m_stTowerInfo.fAtk = float.Parse(InfoMng.Instance.Get_TowerInfo(strTowerFullName, "Atk"));
        m_stTowerInfo.fCri = float.Parse(InfoMng.Instance.Get_TowerInfo(strTowerFullName, "Cri"));
        m_stTowerInfo.eCC = (eCCType)System.Enum.Parse(typeof(eCCType), InfoMng.Instance.Get_TowerInfo(strTowerFullName, "CC"));
        m_stTowerInfo.fCCValue1 = float.Parse(InfoMng.Instance.Get_TowerInfo(strTowerFullName, "CCValue1"));
        m_stTowerInfo.fCCValue2 = float.Parse(InfoMng.Instance.Get_TowerInfo(strTowerFullName, "CCValue2"));
    }
}
