using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMng : MonoBehaviour
{
    static private UIMng instance = null;
    static public UIMng Instance { get { return instance; } }

    private Dictionary<string, GameObject> m_ChildObjDictionary;

    private bool m_bBuildTowerOnFrame = false;

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
        SettingChildObj();
    }

    private void SettingChildObj()
    {
        m_ChildObjDictionary = new Dictionary<string, GameObject>();

        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject ChildObj = transform.GetChild(i).gameObject;
            m_ChildObjDictionary.Add(ChildObj.name, ChildObj);
        }
    }

    public void Set_ChoiceNDSTile(Tile _ChoiceTile)
    {
        if (!_ChoiceTile.bBulid)
        {
            BuildTowerUIOn(_ChoiceTile);
        }
    }

    private void BuildTowerUIOff()
    {
        if (!m_bBuildTowerOnFrame)
        {
            m_ChildObjDictionary["BuildTower"].SetActive(false);
        }
        m_bBuildTowerOnFrame = false;
    }

    private void BuildTowerUIOn(Tile _ChoiceTile)
    {
        if(m_ChildObjDictionary["BuildTower"].activeSelf)
        {
            return;
        }

        if(!m_bBuildTowerOnFrame)
        {
            m_bBuildTowerOnFrame = true;
        }

        m_ChildObjDictionary["BuildTower"].SetActive(true);

        Vector2 v2ChoiceScreenPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out v2ChoiceScreenPos);
        m_ChildObjDictionary["BuildTower"].GetComponent<RectTransform>().localPosition = ScreenPosCalibration(v2ChoiceScreenPos);

        m_ChildObjDictionary["BuildTower"].GetComponent<BuildTower>().ChoiceTile = _ChoiceTile;
    }

    private Vector2 ScreenPosCalibration(Vector2 _v2ChoiceScreenPos)
    {
        Vector2 v2ScreenPosCalibration = _v2ChoiceScreenPos;

        float fWidthHalf = GetComponent<RectTransform>().rect.width * 0.5f;
        float fHeightHalf = GetComponent<RectTransform>().rect.height * 0.5f;

        Rect B_BackGroundRect = m_ChildObjDictionary["BuildTower"].transform.GetChild(0).GetComponent<RectTransform>().rect;

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


    //터치가 일어났을 때 뭔가 하는 함수
    public void Set_OneTouch_Began()
    {
        
    }

    //터치가 끝났을 때 뭔가 하는 함수
    public void Set_OneTouch_Ended()
    {
        if (m_ChildObjDictionary["BuildTower"].activeSelf)
        {
            BuildTowerUIOff();
        }
    }

    public GameObject Get_ChildObj(string _strName)
    {
        return m_ChildObjDictionary[_strName];
    }

    public bool Set_EnemyGoal()
    {
        return m_ChildObjDictionary["Inventory"].GetComponent<Inventory>().Set_EnemyGoal();
    }

    public bool Set_Gold(int _iAddGold)
    {
        return m_ChildObjDictionary["Inventory"].GetComponent<Inventory>().Set_Gold(_iAddGold);
    }
}
