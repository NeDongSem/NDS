using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCCType
{
    Slow,
    Stun,
    eCC_End
}

public struct stTowerInfo
{
    public string strName;
    public string strLevel;
    public float fAPS;
    public float fAtk;
    public float fCri;
    public eCCType eCC;
    public float fCCValue1;
    public float fCCValue2;
}

public class Tower : MonoBehaviour
{
    protected stTowerInfo m_stTowerInfo;
    public stTowerInfo TowerInfo
    {
        set 
        {
            m_stTowerInfo.strName = value.strName; 
            m_stTowerInfo.strLevel = value.strLevel;
            m_stTowerInfo.fAPS = value.fAPS; 
            m_stTowerInfo.fAtk = value.fAtk; 
            m_stTowerInfo.fCri = value.fCri; 
            m_stTowerInfo.eCC = value.eCC; 
            m_stTowerInfo.fCCValue1 = value.fCCValue1; 
            m_stTowerInfo.fCCValue2 = value.fCCValue2; 
        }
    }
    protected Transform m_TargetTransform;
    protected stProjectileInfo m_stProjectileInfo;

    private void Awake()
    {
        Init();
    }

    virtual public void Init()
    {
        m_TargetTransform = null;
        m_stProjectileInfo = new stProjectileInfo();
    }
}
