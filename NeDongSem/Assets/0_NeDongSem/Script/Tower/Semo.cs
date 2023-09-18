using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semo : Tower
{
    public string m_strProjectileName = "SemoProjectile";

    protected override void Shoot()
    {
        base.Shoot();

        GameObject ProjectileGameObject = ObjectPoolMng.Instance.Get_PoolingObject(m_strProjectileName);
        stProjectileInfo stProjectileInfo;
        stProjectileInfo.fAtk = m_stTowerInfo.fAtk;
        stProjectileInfo.fCri = m_stTowerInfo.fCri;
        stProjectileInfo.eCC = m_stTowerInfo.eCC;
        stProjectileInfo.fCCValue1 = m_stTowerInfo.fCCValue1;
        stProjectileInfo.fCCValue2 = m_stTowerInfo.fCCValue2;
        ProjectileGameObject.GetComponent<Projectile>()?.Set_Shoot(transform, m_TargetTransform, stProjectileInfo);
    }
}
