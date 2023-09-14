using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    struct EnemyInfo
    {
        public string strEnemyName;
        public int iHP;
        public int iSpeed;
    }

    EnemyInfo m_EnemyInfo;
    int m_iTileIndex;
    Vector3 m_v3TargetPos;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        
    }

    public void Init()
    {
       
    }

    public void Set_EnemySpawn()
    {
        SettingInfo();
    }

    private void SettingInfo()
    {
        m_EnemyInfo = new EnemyInfo();
        m_EnemyInfo.strEnemyName = "1";
        m_EnemyInfo.iHP = int.Parse(InfoMng.Instance.Get_EnemyInfo(m_EnemyInfo.strEnemyName, "HP"));
        m_EnemyInfo.iSpeed = int.Parse(InfoMng.Instance.Get_EnemyInfo(m_EnemyInfo.strEnemyName, "Speed"));
    }

    public void hit(int _iDmg)
    {
        m_EnemyInfo.iHP -= _iDmg;
        if(m_EnemyInfo.iHP <= 0)
        {
            transform.position = Vector3.zero;
            ObjectPoolMng.Instance.Return_PoolingObject(gameObject, "TestEnemy");
            gameObject.SetActive(false);
        }
    }
}
