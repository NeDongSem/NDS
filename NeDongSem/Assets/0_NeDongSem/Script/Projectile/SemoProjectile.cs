using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemoProjectile : Projectile
{
    protected override void Shoot()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_TargetTransform.position, m_Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.Equals(m_TargetTransform))
        {
            ObjectPoolMng.Instance.Return_PoolingObject(gameObject, m_strName);
        }
    }
}