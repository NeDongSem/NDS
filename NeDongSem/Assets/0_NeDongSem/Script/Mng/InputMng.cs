using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMng : MonoBehaviour
{
    static private InputMng instance = null;
    static public InputMng Instance { get { return instance; } }

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

    Touch[] m_TouchArray;
    Vector2[] m_v2TouchArray;
    public Camera m_MainCamera;
    public float m_CameraMoveSpeed = 3f;
    public float m_CameraZoomSpeed = 2f;
    public Vector2 m_CameraFovMinMax = Vector2.zero;
    Vector2 m_v2TouchCenterPos = Vector2.zero;
    Vector2 m_v2TouchPreCenterPos = Vector2.zero;
    Vector2 m_v2TouchMovePos = Vector2.zero;
    float m_fTouchMag;
    float m_fTouchPreMag;
    float m_fCameraMove;

    private void Init()
    {
        m_TouchArray = new Touch[2];
        m_v2TouchArray = new Vector2[2];
        m_fTouchPreMag = 0f;
    }

    private void Update()
    {
        Touch();

    }

    private void Touch()
    {
        if (Input.touchCount > 0)
        {
            TouchSave();
            if (Input.touchCount == 1)
            {
                TouchUI();
            }
            else
            {
                TouchCalculate();
                TouchCamera();
            }
        }
        else
        {
            TouchReset();
        }
    }

    private void TouchSave()
    {
        for (int i = 0; i < m_TouchArray.Length; ++i)
        {
            m_TouchArray[i] = Input.GetTouch(i);
        }
    }

    private void TouchCalculate()
    {
        for (int i = 0; i < m_v2TouchArray.Length; ++i)
        {
            m_v2TouchArray[i] = m_TouchArray[i].position - m_TouchArray[i].deltaPosition;
        }
    }

    private void TouchUI()
    {

    }

    private void TouchCamera()
    {
        TouchCameraMove();
        TouchCameraZoom();
    }

    private void TouchCameraMove()
    {
        m_v2TouchCenterPos.x = (m_v2TouchArray[0].x + m_v2TouchArray[1].x) * 0.5f;
        m_v2TouchCenterPos.y = (m_v2TouchArray[0].y + m_v2TouchArray[1].y) * 0.5f;

        m_v2TouchMovePos.x = m_v2TouchPreCenterPos.x - m_v2TouchCenterPos.x;
        m_v2TouchMovePos.y = m_v2TouchPreCenterPos.y - m_v2TouchCenterPos.y;

        Vector3 v3MainCameraPos = m_MainCamera.transform.position;
        v3MainCameraPos.x += m_v2TouchMovePos.x * m_CameraMoveSpeed * Time.deltaTime;
        v3MainCameraPos.y += m_v2TouchMovePos.y * m_CameraMoveSpeed * Time.deltaTime;
        m_MainCamera.transform.position = v3MainCameraPos;

        m_v2TouchPreCenterPos = m_v2TouchCenterPos;
    }

    private void TouchCameraZoom()
    {
        m_fTouchMag = (m_v2TouchArray[0] - m_v2TouchArray[1]).magnitude;

        m_fCameraMove = m_fTouchPreMag - m_fTouchMag;

        if (m_MainCamera.orthographic)
        {
            m_MainCamera.orthographicSize += m_fCameraMove * m_CameraZoomSpeed * Time.deltaTime;
            m_MainCamera.orthographicSize = Mathf.Max(m_MainCamera.orthographicSize, m_CameraFovMinMax.x);
        }
        else
        {
            m_MainCamera.fieldOfView += m_fCameraMove * m_CameraZoomSpeed * Time.deltaTime;
            m_MainCamera.fieldOfView = Mathf.Clamp(m_MainCamera.fieldOfView, m_CameraFovMinMax.x, m_CameraFovMinMax.y);
        }

        m_fTouchPreMag = m_fTouchMag;
    }

    private void TouchReset()
    {
        TouchCameraReset();
    }

    private void TouchCameraReset()
    {
        m_fTouchPreMag = 0f;
        m_v2TouchPreCenterPos.x = 0f;
        m_v2TouchPreCenterPos.y = 0f;
    }
}
