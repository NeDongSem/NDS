using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemoProjectile : Projectile
{
    //공격이 끝나면 타워에서 Shoot_End 호출
    protected override void Shoot_Start()
    {
        transform.rotation = m_TowerTransform.rotation;
        transform.position = m_TowerTransform.position;
        transform.parent = m_TowerTransform;
    }

    protected override void Shoot_ing()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.Equals(m_TargetTransform))
        {
            //닿은 적 데미지 && cc 주기(있다면)
            //닿은 위치에서 히트 이펙트 발생시키기
            other.gameObject.GetComponent<Enemy>().hit(2);
        }
    }
}
