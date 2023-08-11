using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolingObject
{
    public float m_Speed;
    protected Transform m_TargetTransform;
    protected bool m_bShooting = false;

    public void Set_Shoot(Transform _TargetTransform, Vector3 _v3TowerPos)
    {
        m_TargetTransform = _TargetTransform;
        transform.position = _v3TowerPos;
        //transform.LookAt(_TargetTransform.position);

        float fRadin = Mathf.Atan2(_TargetTransform.position.y - _v3TowerPos.y, _TargetTransform.position.x - _v3TowerPos.x);
        fRadin *= Mathf.Rad2Deg;
        transform.Rotate(new Vector3(0f, 0f, fRadin));

        m_bShooting = true;
    }

    protected void Update()
    {
        if (m_bShooting)
        {
            Shoot();
        }
    }

    virtual protected void Shoot()
    {

    }

    public override void Set_End()
    {
        m_TargetTransform = null;
        m_bShooting = false;
        base.Set_End();
    }
}
