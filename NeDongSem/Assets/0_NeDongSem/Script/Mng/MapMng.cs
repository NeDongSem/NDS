using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapMng : MonoBehaviour
{
    static private MapMng instance = null;
    static public MapMng Instance { get { return instance; } }

    private void Awake()
    {
        //InstanceCheck
        if (ReferenceEquals(instance, null))
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            Setting();
        }
        else
        {
            Destroy(this.gameObject);
        }

        Set_CreateStageMap();
    }

    public enum TileType
    {
        Edge,
        Enemy,
        NDS,
        Block,
        TileType_End
    }

    public List<Sprite> m_TileSpriteList;
    public GameObject m_CloneTile;
    List<GameObject> m_StageMapTileList;

    private void Setting()
    {
        m_StageMapTileList = new List<GameObject>();
    }

    public void Set_CreateStageMap()
    {
        string[] CSVTextLineArray = Get_NowStageMapCSV();
        Setting_StageMapTileList(CSVTextLineArray.Length);
    }

    public string[] Get_NowStageMapCSV()
    {
        string SceneName = SceneManager.GetActiveScene().name;
        string CSVPath = Application.dataPath + "/0_NeDongSem/Etc/CSV/Map/" + SceneName + ".csv";
        return CSVMng.Instance.Get_CSVTextLineArray(CSVPath);
    }

    private void Setting_StageMapTileList(int _iSize)
    {
        int StageMapTileListCount = m_StageMapTileList.Count;

        if(StageMapTileListCount < _iSize)
        {
            int AddCount = _iSize - StageMapTileListCount;

            for(int i = 0; i < AddCount; ++i)
            {
                //리스트 만들어서 넣고 게임 오브젝트 인덱스 이름으로 하나 만들어서 클론 타일 자식으로 넣기
            }
        }
    }

    public void CreateTileLine(int _iLineIndex, int _iSizeX, int _iSizeY)
    {

    }
}
