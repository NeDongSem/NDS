using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct stProjectileInfo
{
    public float Atk;
    public float Cri;
    public eCCType CC;
    public float CCValue;
}

public class Projectile : PoolingObject
{
    protected Transform m_TowerTransform;
    protected Transform m_TargetTransform;
    protected stProjectileInfo m_stProjectileInfo;
    protected bool m_bShooting = false;

    public void Set_Shoot(Transform _TowerTransform, Transform _TargetTransform , stProjectileInfo _stProjectileInfo)
    {
        m_TowerTransform = _TowerTransform;
        m_TargetTransform = _TargetTransform;
        transform.position = _TowerTransform.position;
        m_stProjectileInfo = _stProjectileInfo;
        m_bShooting = true;
        Shoot_Start();
    }

    protected void Update()
    {
        if (m_bShooting)
        {
            Shoot_ing();
        }
    }

    virtual protected void Shoot_Start()
    {

    }

    virtual protected void Shoot_ing()
    {

    }

    public override void Set_End()
    {
        m_TargetTransform = null;
        m_bShooting = false;
        base.Set_End();
    }

    virtual public void Shoot_End(bool _bHit = true)
    {
        ObjectPoolMng.Instance.Return_PoolingObject(gameObject, m_strName);
    }
}
