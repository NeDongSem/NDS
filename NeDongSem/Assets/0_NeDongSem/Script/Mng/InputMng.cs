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
    public Camera m_MainCamera;
    public float m_CameraMoveSpeed = 3f;
    public float m_CameraZoomSpeed = 2f;
    public Vector2 m_CameraMoveMinMaxPosX = Vector2.zero;
    public Vector2 m_CameraMoveMinMaxPosY = Vector2.zero;
    public Vector2 m_CameraFovMinMax = Vector2.zero;
    Vector2 m_v2TouchCenterPos = Vector2.zero;
    Vector2 m_v2TouchPreCenterPos = Vector2.zero;
    Vector2 m_v2TouchMovePos = Vector2.zero;
    float m_fTouchMag;
    float m_fTouchPreMag;
    float m_fCameraZoom;

    private void Init()
    {
        m_TouchArray = new Touch[2];
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
        if(m_v2TouchPreCenterPos.x <= 0f && m_v2TouchPreCenterPos.y <= 0f)
        {
            m_v2TouchPreCenterPos.x = (m_TouchArray[0].position.x + m_TouchArray[1].position.x) * 0.5f;
            m_v2TouchPreCenterPos.y = (m_TouchArray[0].position.y + m_TouchArray[1].position.y) * 0.5f;
            return;
        }

        m_v2TouchCenterPos.x = (m_TouchArray[0].position.x + m_TouchArray[1].position.x) * 0.5f;
        m_v2TouchCenterPos.y = (m_TouchArray[0].position.y + m_TouchArray[1].position.y) * 0.5f;

        m_v2TouchMovePos.x = m_v2TouchPreCenterPos.x - m_v2TouchCenterPos.x;
        m_v2TouchMovePos.y = m_v2TouchPreCenterPos.y - m_v2TouchCenterPos.y;

        Vector3 v3MainCameraPos = m_MainCamera.transform.position;
        v3MainCameraPos.x += m_v2TouchMovePos.x * m_CameraMoveSpeed * Time.deltaTime;
        v3MainCameraPos.y += m_v2TouchMovePos.y * m_CameraMoveSpeed * Time.deltaTime;

        if(v3MainCameraPos.x < m_CameraMoveMinMaxPosX.x)
        {
            v3MainCameraPos.x = m_CameraMoveMinMaxPosX.x;
        }
        else if(v3MainCameraPos.x > m_CameraMoveMinMaxPosX.y)
        {
            v3MainCameraPos.x = m_CameraMoveMinMaxPosX.y;
        }

        if (v3MainCameraPos.y < m_CameraMoveMinMaxPosY.x)
        {
            v3MainCameraPos.y = m_CameraMoveMinMaxPosY.x;
        }
        else if (v3MainCameraPos.y > m_CameraMoveMinMaxPosY.y)
        {
            v3MainCameraPos.y = m_CameraMoveMinMaxPosY.y;
        }

        m_MainCamera.transform.position = v3MainCameraPos;

        m_v2TouchPreCenterPos = m_v2TouchCenterPos;
    }

    private void TouchCameraZoom()
    {
        if(m_fTouchPreMag <= 0f)
        {
            m_fTouchPreMag = (m_TouchArray[0].position - m_TouchArray[1].position).magnitude;
            return;
        }

        m_fTouchMag = (m_TouchArray[0].position - m_TouchArray[1].position).magnitude;

        m_fCameraZoom = m_fTouchPreMag - m_fTouchMag;

        if (m_MainCamera.orthographic)
        {
            m_MainCamera.orthographicSize += m_fCameraZoom * m_CameraZoomSpeed * Time.deltaTime;
            m_MainCamera.orthographicSize = Mathf.Max(m_MainCamera.orthographicSize, m_CameraFovMinMax.x);
        }
        else
        {
            m_MainCamera.fieldOfView += m_fCameraZoom * m_CameraZoomSpeed * Time.deltaTime;
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
        m_fTouchPreMag = -1f;
        m_v2TouchPreCenterPos.x = -1f;
        m_v2TouchPreCenterPos.y = -1f;
    }
}
