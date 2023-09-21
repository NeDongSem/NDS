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
        transform.Rotate(new Vector3(0f, 0f, fRadin + 180f));

        m_TargetTransform.GetComponent<Enemy>()?.Set_AddAtkMeProjectileList(this);
    }

    protected override void Shoot_ing()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_TargetTransform.position, m_Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.Equals(m_TargetTransform))
        {
            int iCri = Random.Range(0, 100);

            if(iCri < m_stProjectileInfo.fCri)
            {
                other.gameObject.GetComponent<Enemy>()?.Set_Hit((int)(m_stProjectileInfo.fAtk * 2f));
            }
            else
            {
                other.gameObject.GetComponent<Enemy>()?.Set_Hit((int)(m_stProjectileInfo.fAtk));
            }


            if (m_stProjectileInfo.eCC == eCCType.Stun || m_stProjectileInfo.eCC == eCCType.Slow)
            {
                other.gameObject.GetComponent<Enemy>()?.Set_CC(m_stProjectileInfo.eCC, m_stProjectileInfo.fCCValue1, m_stProjectileInfo.fCCValue2);
            }

            other.gameObject.GetComponent<Enemy>()?.Set_RemoveAtkMeProjectileList(this);

            Shoot_End();
        }
    }
}
