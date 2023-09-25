using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    private Enemy m_Enemy;
    public Enemy EnemyObj
    {
        get { return m_Enemy; }
        set 
        { 
            m_Enemy = value;
            GetComponent<RectTransform>().SetParent(UIMng.Instance.Get_ChildObj("EnemyHpBarList").GetComponent<RectTransform>());
            GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            HpBar_Pos();
            HpBar_Value();
            gameObject.SetActive(true);
        }
    }

    public Slider m_HpBarSlider;

    private void Update()
    {
        HpBar_Update();
    }

    private void HpBar_Update()
    {
        HpBar_Pos();
        HpBar_Value();
    }

    private void HpBar_Pos()
    {
        Vector3 v3ScreenPos = Camera.main.WorldToScreenPoint(m_Enemy.transform.position);

        if (v3ScreenPos.z < 0.0f)
        {
            v3ScreenPos *= -1.0f;
        }

        Vector2 v2LocalPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMng.Instance.GetComponent<RectTransform>(), v3ScreenPos, Camera.main, out v2LocalPos);

        GetComponent<RectTransform>().localPosition = v2LocalPos;
    }

    private void HpBar_Value()
    {
        m_HpBarSlider.value = m_Enemy.HpRatio;
    }

    public void Set_Return()
    {
        GetComponent<RectTransform>().SetParent(null);
        GetComponent<RectTransform>().localPosition = Vector2.zero;
        ObjectPoolMng.Instance.Return_PoolingObject(gameObject, "EnemyHpBar");
        gameObject.SetActive(false);
    }
}
