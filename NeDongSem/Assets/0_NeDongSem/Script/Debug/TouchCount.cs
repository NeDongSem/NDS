using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TouchCount : MonoBehaviour
{
    public TextMeshProUGUI TouchCounttxt;
    public TextMeshProUGUI m_v2TouchCenterPosX;
    public TextMeshProUGUI m_v2TouchCenterPosY;
    public TextMeshProUGUI m_v2TouchPreCenterPosX;
    public TextMeshProUGUI m_v2TouchPreCenterPosY;
    public TextMeshProUGUI m_v2TouchMovePosX;
    public TextMeshProUGUI m_v2TouchMovePosY;
    public TextMeshProUGUI m_fTouchMag;
    public TextMeshProUGUI m_fTouchPreMag;
    public TextMeshProUGUI m_fCameraZoom;
    public TextMeshProUGUI m_Dbug;
    public TextMeshProUGUI m_ChoiceObjPosX;
    public TextMeshProUGUI m_ChoiceObjPosY;
    public TextMeshProUGUI m_ChoiceObjPosZ;

    // Update is called once per frame
    void Update()
    {
        TouchCounttxt.text = Input.touchCount.ToString();
        m_v2TouchCenterPosX.text = InputMng.Instance.TouchCenterPos.x.ToString();
        m_v2TouchCenterPosY.text = InputMng.Instance.TouchCenterPos.y.ToString();
        m_v2TouchPreCenterPosX.text = InputMng.Instance.TouchPreCenterPos.x.ToString();
        m_v2TouchPreCenterPosY.text = InputMng.Instance.TouchPreCenterPos.y.ToString();
        m_v2TouchMovePosX.text = InputMng.Instance.TouchMovePos.x.ToString();
        m_v2TouchMovePosY.text = InputMng.Instance.TouchMovePos.y.ToString();
        m_fTouchMag.text = InputMng.Instance.TouchMag.ToString();
        m_fTouchPreMag.text = InputMng.Instance.TouchPreMag.ToString();
        m_fCameraZoom.text = InputMng.Instance.CameraZoom.ToString();
        m_Dbug.text = InputMng.Instance.Dbug;

        m_ChoiceObjPosX.text = InputMng.Instance.ChoiceObjPos.x.ToString();
        m_ChoiceObjPosY.text = InputMng.Instance.ChoiceObjPos.y.ToString();
        m_ChoiceObjPosZ.text = InputMng.Instance.ChoiceObjPos.z.ToString();

    }
}
