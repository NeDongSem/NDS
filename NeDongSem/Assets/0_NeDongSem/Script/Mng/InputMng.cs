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

    Vector3 m_v3ChoiceObjPos;

    private void Init()
    {
        m_TouchArray = new Touch[2];
        m_fTouchPreMag = 0f;
        //StartCoroutine("ScreenRotate");
    }

    private void Update()
    {
#if UNITY_EDITOR
        ClickObj();
#else
        Touch();
#endif
    }

    private void ClickObj()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OneTouch_Began();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OneTouch_Ended();
        }
    }

    private void ChoiceObj()
    {
        Ray ChoiceRay = Camera.main.ScreenPointToRay(m_v3ChoiceObjPos);
        RaycastHit[] RaycastHitArry;

        RaycastHitArry = Physics.RaycastAll(ChoiceRay, (Camera.main.transform.position.z * -2f),LayerMask.GetMask("Tile"));

        if(RaycastHitArry.Length > 0)
        {
            GameObject FrontGameObject = RaycastHitArry[RaycastHitArry.Length - 1].transform.gameObject;
            Tile ChoiceTile = FrontGameObject.GetComponent<Tile>();
            if (!ReferenceEquals(ChoiceTile, null))
            {
                if(ChoiceTile.TileType == eTileType.NDS)
                {
                    UIMng.Instance.Set_ChoiceNDSTile(ChoiceTile);
                }
            }
        }
    }

    private void Touch()
    {
        if (Input.touchCount > 0)
        {
            TouchSave();
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                OneTouch_Began();
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                OneTouch_Ended();
            }
        }
        else
        {
            TouchReset();
        }
    }

    //손가락을 댈 때
    private void OneTouch_Began()
    {
#if UNITY_EDITOR
        TouchObj(); //마우스는 어짜피 원 터치라
#else
        if (Input.touchCount == 1)
        {
            TouchObj();
        }
        else
        {
            TouchCamera();
        }
#endif
        UIMng.Instance.Set_OneTouch_Began();
    }

    //손가락을 뗄 때
    private void OneTouch_Ended()
    {
        UIMng.Instance.Set_OneTouch_Ended();
    }

    private void TouchSave()
    {
        for (int i = 0; i < m_TouchArray.Length; ++i)
        {
            m_TouchArray[i] = Input.GetTouch(i);
        }
    }

    private void TouchObj()
    {
#if UNITY_EDITOR
        m_v3ChoiceObjPos = Input.mousePosition;
#else
        m_v3ChoiceObjPos = Input.GetTouch(0).position;
#endif
        ChoiceObj();
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

    //IEnumerator ScreenRotate()
    //{
    //    bool bLandscapeRight;
    //    if(Screen.orientation == ScreenOrientation.LandscapeRight)
    //    {
    //        bLandscapeRight = true;
    //    }
    //    else
    //    {
    //        bLandscapeRight = false;
    //    }

    //    while(true)
    //    {
    //        if(bLandscapeRight)
    //        {
    //            if(Input.acceleration.y > 0.5f)
    //            {
    //                    Screen.orientation = ScreenOrientation.LandscapeLeft;
    //                    bLandscapeRight = false;
    //            }
    //        }
    //        else
    //        {
    //            if (Input.acceleration.y <= 0.5f)
    //            {
    //                    Screen.orientation = ScreenOrientation.LandscapeRight;
    //                    bLandscapeRight = true;
    //            }
    //        }

    //        yield return new WaitForSecondsRealtime(0.5f);
    //    }
    //}
}
