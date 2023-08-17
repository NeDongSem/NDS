using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemoProjectile : Projectile
{
    public float m_Speed;

    protected override void Shoot_Start()
    {
        float fRadin = Mathf.Atan2(m_TargetTransform.position.y - transform.position.y, m_TargetTransform.position.x - transform.position.x);
        fRadin *= Mathf.Rad2Deg;
        transform.Rotate(new Vector3(0f, 0f, fRadin));
    }

    protected override void Shoot_ing()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_TargetTransform.position, m_Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.Equals(m_TargetTransform))
        {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            e.hit(1);
            Shoot_End();
        }
    }
}
