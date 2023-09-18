using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCCType
{
    None,
    Slow,
    Stun,
    eCC_End
}

public struct stTowerInfo
{
    public string strName;
    public string strLevel;
    public eTowerType eTowerType;
    public eTowerLevel eTowerLevel;
    public float fDis;
    public float fAps;
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
            m_stTowerInfo.eTowerType = value.eTowerType;
            m_stTowerInfo.eTowerLevel = value.eTowerLevel;
            m_SpriteRenderer.sprite = TowerSpwanMng.Instance.Get_TowerSprite(m_stTowerInfo.eTowerType, m_stTowerInfo.eTowerLevel);
            m_stTowerInfo.fDis = value.fDis;
            m_stTowerInfo.fAps = value.fAps;
            m_fFireDelayTime = 1f / m_stTowerInfo.fAps;
            m_stTowerInfo.fAtk = value.fAtk; 
            m_stTowerInfo.fCri = value.fCri; 
            m_stTowerInfo.eCC = value.eCC; 
            m_stTowerInfo.fCCValue1 = value.fCCValue1; 
            m_stTowerInfo.fCCValue2 = value.fCCValue2; 
        }
    }

    public SpriteRenderer m_SpriteRenderer;

    protected Transform m_TargetTransform;
    protected stProjectileInfo m_stProjectileInfo;
    protected float m_fFireDelayTime;
    protected float m_fTime;

    protected void Awake()
    {
        Init();
    }

    protected void Update()
    {
        EnemyTargeting();
        ShootingCheck();
    }

    virtual public void Init()
    {
        m_stTowerInfo = new stTowerInfo();
        m_TargetTransform = null;
        m_stProjectileInfo = new stProjectileInfo();
    }

    protected void EnemyTargeting()
    {
        if(Vector3.Distance(transform.position, m_TargetTransform.position) > m_stTowerInfo.fDis)
        {
            m_TargetTransform = null;
        }

        if(m_TargetTransform == null)
        {
            Collider[] HitColliders = Physics.OverlapSphere(transform.position, m_stTowerInfo.fDis, LayerMask.GetMask("Enemy"));
            float fDisTemp = m_stTowerInfo.fDis + 1f;
            foreach (Collider Collider in HitColliders)
            {
                float fDis = Vector3.Distance(transform.position, Collider.transform.position);
                //가장 가까운 적을 타겟팅
                if(fDis < fDisTemp)
                {
                    fDisTemp = fDis;
                    m_TargetTransform = Collider.transform;
                }
            }
        }
    }

    protected void ShootingCheck()
    {
        if(m_TargetTransform == null)
        {
            m_fTime = 0f;
            return;
        }

        m_fTime += Time.deltaTime;
        if(m_fTime >= m_fFireDelayTime)
        {
            m_fTime = 0f;
            Shoot();
        }
    }

    virtual protected void Shoot()
    {
        //각 타워가 재정의 할 것
    }
}
