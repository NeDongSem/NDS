using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireTest : MonoBehaviour
{
    public string m_ProjectileName;
    public Transform m_Target;
    public float m_FireTime;

    float m_fTime = 0f;

    private void Update()
    {
        if (m_fTime >= m_FireTime)
        {
            GameObject Projectile = ObjectPoolMng.Instance.Get_PoolingObject(m_ProjectileName);
            Projectile.GetComponent<Projectile>().Set_Shoot(m_Target, transform.position);
            m_fTime = 0f;
        }
        else
        {
            m_fTime += Time.deltaTime;
        }
    }
}
