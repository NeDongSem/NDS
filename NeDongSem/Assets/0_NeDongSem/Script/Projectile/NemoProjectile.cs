using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemoProjectile : Projectile
{
    //������ ������ Ÿ������ Shoot_End ȣ��
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
            //���� �� ������ && cc �ֱ�(�ִٸ�)
            //���� ��ġ���� ��Ʈ ����Ʈ �߻���Ű��
            other.gameObject.GetComponent<Enemy>().hit(2);
        }
    }
}
