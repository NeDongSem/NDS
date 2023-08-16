using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int iHp = 10;

    public void hit(int _iDmg)
    {
        iHp -= _iDmg;
        if(iHp <= 0)
        {
            transform.position = Vector3.zero;
            ObjectPoolMng.Instance.Return_PoolingObject(gameObject, "TestEnemy");
            gameObject.SetActive(false);
        }
    }
}
