using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DongramiProjectile : Projectile
{
    public float m_Speed;
    public float m_ExplosionRange;
    Vector3 m_v3TargetPos = Vector3.zero;
    int m_iLayerMask;

    public override void Init()
    {
        base.Init();
        m_iLayerMask = LayerMask.GetMask("Enemy"); /*에너미 레이어 마스크 너어야 함*/
    }

    protected override void Shoot_Start()
    {
        m_v3TargetPos.x = m_TargetTransform.position.x;
        m_v3TargetPos.y = m_TargetTransform.position.y;
        m_v3TargetPos.z = m_TargetTransform.position.z;
    }

    protected override void Shoot_ing()
    {
        if(Vector3.Distance(transform.position, m_v3TargetPos) < 0.01f)
        {
            Explosion();
            Shoot_End();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, m_v3TargetPos, m_Speed * Time.deltaTime);
        }
    }

    private void Explosion()
    {
        //터지는 거 빠아아ㅏㅇ 이펙트 발동, 주변 데미지 주기 발똥
        ExplosionEff();
        RangedAttack();
    }

    private void ExplosionEff()
    {
    }

    private void RangedAttack()
    {
        Collider[] HitColliders = Physics.OverlapSphere(transform.position, m_ExplosionRange, m_iLayerMask);
        foreach (Collider Collider in HitColliders)
        {
            //if (Collider.name == gameObject.name /* 자기 자신은 제외 */) 
            //    continue; /* 이 친구는 콜라이더를 안 가지고 있을 예정 */

            Enemy enemy = Collider.gameObject.GetComponent<Enemy>();
            if(enemy != null)
            {
                //enemy.hit(m_stProjectileInfo.Damage);
                enemy.Set_Hit(2);
            }
        }
    }
}
