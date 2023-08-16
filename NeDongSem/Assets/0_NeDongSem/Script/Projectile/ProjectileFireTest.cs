using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireTest : MonoBehaviour
{
    public string m_ProjectileName;
    public Transform m_Target;
    public float m_FireTime;
    public float m_Speed;
    public float m_Damage;
    public eCCType m_eCCType = eCCType.eCC_End;
    public float m_CCValue;
    float m_fTime = 0f;
    GameObject m_CurrentProjectileGameObject;
    private void Update()
    {
        if (m_Target != null)
        {
            if(Vector3.Distance(transform.position, m_Target.position) > 100f)
            {
                m_Target = null;
                m_fTime = 0f;
                return;
            }

            if (m_fTime >= m_FireTime)
            {
                m_CurrentProjectileGameObject = ObjectPoolMng.Instance.Get_PoolingObject(m_ProjectileName);
                stProjectileInfo stProjectileInfo;
                stProjectileInfo.Speed = m_Speed;
                stProjectileInfo.Damage = m_Damage;
                stProjectileInfo.CCType = m_eCCType;
                stProjectileInfo.CCValue = m_CCValue;
                m_CurrentProjectileGameObject.GetComponent<Projectile>().Set_Shoot(transform, m_Target, stProjectileInfo);
                if (m_ProjectileName == "NemoProjectile")
                {
                    StartCoroutine("NemoProjectileTimer");
                }
                m_fTime = 0f;
            }
            else
            {
                m_fTime += Time.deltaTime;
            }
        }
        else
        {
            Collider[] HitColliders = Physics.OverlapSphere(transform.position, 100, LayerMask.GetMask("Enemy"));
            foreach (Collider Collider in HitColliders)
            {
                m_Target = Collider.transform;
                break;
            }
        }
    }

    private IEnumerator NemoProjectileTimer()
    {
        yield return new WaitForSeconds(m_FireTime * 0.5f);
        m_CurrentProjectileGameObject.GetComponent<Projectile>().Shoot_End();
    }
}
