using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTower : MonoBehaviour
{
    private Tile m_ChoiceTile;
    public Tile ChoiceTile
    {
        get { return m_ChoiceTile; }
        set { m_ChoiceTile = value; }
    }

    public void Set_SpawnTower_Nemo()
    {
        if (ReferenceEquals(m_ChoiceTile, null))
        {
            return;
        }

        TowerSpwanMng.Instance.Set_TowerSpwan(m_ChoiceTile, eTowerType.Nemo);
        m_ChoiceTile = null;
    }

    public void Set_SpawnTower_Dongrami()
    {
        if (ReferenceEquals(m_ChoiceTile, null))
        {
            return;
        }

        TowerSpwanMng.Instance.Set_TowerSpwan(m_ChoiceTile, eTowerType.Dongrami);
        m_ChoiceTile = null;
    }

    public void Set_SpawnTower_Semo()
    {
        if(ReferenceEquals(m_ChoiceTile,null))
        {
            return;
        }

        TowerSpwanMng.Instance.Set_TowerSpwan(m_ChoiceTile, eTowerType.Semo);
        m_ChoiceTile = null;
    }

    public void Set_SpawnTower_Cansel()
    {

    }
}
