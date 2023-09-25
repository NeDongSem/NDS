using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    struct EnemyInfo
    {
        public string strEnemyName;
        public int iHp;
        public int iSpeed;
        public eCCType eCC;
        public float fCCValue1;
        public float fCCValue2;
        public int iGold;
    }

    EnemyInfo m_stEnemyInfo;
    int m_iMaxHp;
    public float HpRatio
    {
        get { return (float)m_stEnemyInfo.iHp / (float)m_iMaxHp; }
    }
    int m_iTileIndex;
    Vector3 m_v3TargetPos;
    float m_fTime;

    List<Projectile> m_AtkMeProjectileList;

    EnemyHpBar m_EnemyHpBar;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        Move();
    }

    public void Init()
    {
        m_AtkMeProjectileList = new List<Projectile>();
    }

    public void Set_EnemySpawn(string _strEnemyName)
    {
        SettingInfo(_strEnemyName);
        m_AtkMeProjectileList.Clear();
        SettingHpBar();
    }

    private void SettingInfo(string _strEnemyName)
    {
        m_stEnemyInfo = new EnemyInfo();
        m_stEnemyInfo.strEnemyName = _strEnemyName;
        m_stEnemyInfo.iHp = int.Parse(InfoMng.Instance.Get_EnemyInfo(m_stEnemyInfo.strEnemyName, "Hp"));
        m_iMaxHp = m_stEnemyInfo.iHp;
        m_stEnemyInfo.iSpeed = int.Parse(InfoMng.Instance.Get_EnemyInfo(m_stEnemyInfo.strEnemyName, "Speed"));
        m_stEnemyInfo.eCC = eCCType.None;
        m_stEnemyInfo.fCCValue1 = 0f;
        m_stEnemyInfo.fCCValue2 = 0f;
        m_stEnemyInfo.iGold = int.Parse(InfoMng.Instance.Get_EnemyInfo(m_stEnemyInfo.strEnemyName, "Gold"));
        m_iTileIndex = 0;
        m_v3TargetPos = MapMng.Instance.EnemyTileList[m_iTileIndex].transform.position;
        m_v3TargetPos.z = transform.position.z;
        m_fTime = 0f;
    }

    private void SettingHpBar()
    {
        m_EnemyHpBar = ObjectPoolMng.Instance.Get_PoolingObject("EnemyHpBar").GetComponent<EnemyHpBar>();
        m_EnemyHpBar.EnemyObj = this;
    }

    private void Move()
    {
        if(m_stEnemyInfo.eCC == eCCType.Stun)
        {
            m_fTime += Time.deltaTime;
            if(m_fTime >= m_stEnemyInfo.fCCValue1)
            {
                m_fTime = 0f;
                m_stEnemyInfo.eCC = eCCType.None;
            }
            return;
        }

        float fSpeed = (float)m_stEnemyInfo.iSpeed;
        if (m_stEnemyInfo.eCC == eCCType.Slow)
        {
            m_fTime += Time.deltaTime;
            if (m_fTime >= m_stEnemyInfo.fCCValue1)
            {
                m_fTime = 0f;
                m_stEnemyInfo.eCC = eCCType.None;
            }
            else
            {
                fSpeed *= m_stEnemyInfo.fCCValue2;
            }
        }

        float fMoveDis = Time.deltaTime * fSpeed;
        float fRemainingDis = Vector3.Distance(transform.position, m_v3TargetPos);
        if(fMoveDis >= fRemainingDis)
        {
            if(++m_iTileIndex == MapMng.Instance.EnemyTileList.Count)
            {
                Goal();
            }
            else
            {
                fMoveDis -= fRemainingDis;
                transform.position = m_v3TargetPos;
                m_v3TargetPos = MapMng.Instance.EnemyTileList[m_iTileIndex].transform.position;
                m_v3TargetPos.z = transform.position.z;
            }
        }

        Vector3 v3Dir = m_v3TargetPos - transform.position;
        v3Dir = v3Dir.normalized;

        Vector3 v3Pos = transform.position + (v3Dir* fMoveDis);
        transform.position = v3Pos;
    }

    private void Goal()
    {
        Return();
        UIMng.Instance.Set_EnemyGoal();
    }

    public void Set_Hit(int _iDmg)
    {
        m_stEnemyInfo.iHp -= _iDmg;
        if(m_stEnemyInfo.iHp <= 0)
        {
            UIMng.Instance.Set_Gold(m_stEnemyInfo.iGold);
            Return();
        }
    }

    public void Set_CC(eCCType _eCCType, float _fCCValue1, float _fCCValue2)
    {
        //기존에 걸린 cc가 잇다면 그 다음껀 무시
        if(m_stEnemyInfo.eCC == _eCCType)
        {
            return;
        }

        m_stEnemyInfo.eCC = _eCCType;
        m_stEnemyInfo.fCCValue1 = _fCCValue1;
        m_stEnemyInfo.fCCValue2 = _fCCValue2;
    }

    public void Set_AddAtkMeProjectileList(Projectile _Projectile)
    {
        m_AtkMeProjectileList.Add(_Projectile);
    }

    public void Set_RemoveAtkMeProjectileList(Projectile _Projectile)
    {
        for(int i = 0; i < m_AtkMeProjectileList.Count; ++i)
        {
            if(ReferenceEquals(m_AtkMeProjectileList[i], _Projectile))
            {
                m_AtkMeProjectileList.RemoveAt(i);
                return;
            }
        }
    }

    private void Return()
    {
        for (int i = 0; i < m_AtkMeProjectileList.Count; ++i)
        {
            m_AtkMeProjectileList[i].Shoot_End(false);
        }

        m_EnemyHpBar.Set_Return();

        transform.position = Vector3.zero;
        ObjectPoolMng.Instance.Return_PoolingObject(gameObject, "Enemy");
        gameObject.SetActive(false);
    }

}
