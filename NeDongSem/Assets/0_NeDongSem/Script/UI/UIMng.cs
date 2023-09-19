using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMng : MonoBehaviour
{
    static private UIMng instance = null;
    static public UIMng Instance { get { return instance; } }

    public GameObject m_BuildTowerUI;

    private void Awake()
    {
        //InstanceCheck
        if (ReferenceEquals(instance, null))
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Init()
    {
    }

    public void Set_ChoiceNDSTile(Vector3 _v3ChoicePos, Tile _ChoiceTile)
    {
        BuildTowerUI(_v3ChoicePos);
    }

    private void BuildTowerUI()
    {
        m_BuildTowerUI.SetActive(false);
    }

    private void BuildTowerUI(Vector3 _v3ChoicePos)
    {
        m_BuildTowerUI.SetActive(true);

        Vector2 v2ChoiceScreenPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out v2ChoiceScreenPos);
        m_BuildTowerUI.GetComponent<RectTransform>().localPosition = ScreenPosCalibration(v2ChoiceScreenPos);
    }

    private Vector2 ScreenPosCalibration(Vector2 _v2ChoiceScreenPos)
    {
        Vector2 v2ScreenPosCalibration = _v2ChoiceScreenPos;

        float fWidthHalf = GetComponent<RectTransform>().rect.width * 0.5f;
        float fHeightHalf = GetComponent<RectTransform>().rect.height * 0.5f;

        Rect B_BackGroundRect = m_BuildTowerUI.transform.GetChild(0).GetComponent<RectTransform>().rect;

        if (((_v2ChoiceScreenPos.x - B_BackGroundRect.width) < (fWidthHalf * -1f)))
        {
            v2ScreenPosCalibration.x += B_BackGroundRect.width;
        }
        if (((_v2ChoiceScreenPos.y - B_BackGroundRect.height) < (fHeightHalf * -1f)))
        {
            v2ScreenPosCalibration.y += B_BackGroundRect.height;
        }

        return v2ScreenPosCalibration;
    }


}
