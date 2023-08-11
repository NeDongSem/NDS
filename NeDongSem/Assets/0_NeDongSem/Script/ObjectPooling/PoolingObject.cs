using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    protected string m_strName = "";
    protected bool m_bUsing = false;

    protected Vector3 m_v3BasePos;
    protected Vector3 m_v3BaseScale;
    protected Vector3 m_v3BaseRot;

    protected void Awake()
    {
        Init();
    }

    virtual public void Init()
    {
        BaseSettingSave();
    }

    virtual protected void BaseSettingSave()
    {
        m_v3BasePos = new Vector3();
        m_v3BaseScale = new Vector3();
        m_v3BaseRot = new Vector3();

        m_v3BasePos = transform.position;
        m_v3BaseScale = transform.localScale;
        m_v3BaseRot = transform.rotation.eulerAngles;
    }

    public void Set_Name(string _strName)
    {
        m_strName = _strName;
    }

    virtual public void Set_Ready()
    {
        //매니저에서 호출해주는 함수, 필요하다면 재정의
        gameObject.SetActive(true);
        ResetAll();
        m_bUsing = true;
    }

    virtual public void Set_End()
    {
        //매니저에서 호출해주는 함수, 필요하다면 재정의
        m_bUsing = false;
        ResetAll();
        gameObject.SetActive(false);
    }

    protected void ResetAll()
    {
        Reset_Parent();
        Reset_Pos();
        Reset_Scale();
        Reset_Rot();
    }

    protected void Reset_Parent()
    {
        transform.parent = null;
    }

    protected void Reset_Pos()
    {
        transform.position = m_v3BasePos;
    }

    protected void Reset_Scale()
    {
        transform.localScale = m_v3BaseScale;
    }
    protected void Reset_Rot()
    {
        transform.rotation = Quaternion.Euler(m_v3BaseRot);
    }
}
