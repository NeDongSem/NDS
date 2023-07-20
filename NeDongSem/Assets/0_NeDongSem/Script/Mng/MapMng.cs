using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapMng : MonoBehaviour
{
    static private MapMng instance = null;
    static public MapMng Instance { get { return instance; } }

    public enum eTileType
    {
        Edge,
        Enemy,
        NDS,
        Block,
        TileType_End
    }

    public List<Sprite> m_TileSpriteList;
    public GameObject m_Tile;
    public GameObject m_CloneTile;
    List<List<GameObject>> m_StageMapTileList;
    int m_iEnemyTileCount;
    List<GameObject> m_EnemyTileList;

    struct stTileSize
    {
        int iTileSizeX;
        public int X
        {
            get { return iTileSizeX; }
            set { iTileSizeX = value; }
        }
        int iTileSizeY;
        public int Y
        {
            get { return iTileSizeY; }
            set { iTileSizeY = value; }
        }
    }
    stTileSize TileSize;


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

        Set_CreateStageMap(32,32);
    }

    private void Setting()
    {
        m_StageMapTileList = new List<List<GameObject>>();
        m_EnemyTileList = new List<GameObject>();
    }

    public void Set_CreateStageMap(int _iTileSizeX = 0, int _iTileSizeY = 0)
    {
        if (_iTileSizeX != 0)
        {
            TileSize.X = _iTileSizeX;
        }
        if(_iTileSizeY != 0)
        {
            TileSize.Y = _iTileSizeY;
        }

        string[] CSVTextLineArray = Get_NowStageMapCSV();
        EnemyTile_Init(CSVTextLineArray);
        int iTileCountX = CSVMng.Instance.Get_CSVTextDivision(CSVTextLineArray[0], ",").Length;
        CreateStageMap_Init(CSVTextLineArray.Length, iTileCountX);
        CreateStageMap_Setting(CSVTextLineArray, CSVTextLineArray.Length, iTileCountX);
    }

    private void EnemyTile_Init(string[] _CSVTextLineArray)
    {
        m_iEnemyTileCount = 0;
        m_EnemyTileList.Clear();
        for (int i = 0; i < _CSVTextLineArray.Length; ++i)
        {
            string[] TileLineArray = _CSVTextLineArray[i].Split(",");
            for (int j = 0; j < TileLineArray.Length; ++j)
            {
                if((TileLineArray[j] != "") && (TileLineArray[j][0] == '1'))
                {
                    string EnemyTile = TileLineArray[j];
                    EnemyTile = EnemyTile.Substring(2, EnemyTile.Length - 3);
                    string[] EnemyTileNum = EnemyTile.Split("&");

                    for(int k = 0; k < EnemyTileNum.Length; ++k)
                    {
                        if (int.Parse(EnemyTileNum[k]) > m_iEnemyTileCount)
                        {
                            m_iEnemyTileCount = int.Parse(EnemyTileNum[k]);
                        }
                    }
                }
            }
        }

        ++m_iEnemyTileCount;

        if (m_iEnemyTileCount > m_EnemyTileList.Count)
        {
            while ((m_iEnemyTileCount - m_EnemyTileList.Count) > 0)
            {
                m_EnemyTileList.Add(null);
            }
        }
        else if (m_iEnemyTileCount < m_EnemyTileList.Count)
        {
            while (m_EnemyTileList.Count - m_iEnemyTileCount > 0)
            {
                m_EnemyTileList.RemoveAt(m_EnemyTileList.Count - 1);
            }
        }
    }

    public string[] Get_NowStageMapCSV()
    {
        string SceneName = SceneManager.GetActiveScene().name;
        string CSVPath = Application.dataPath + "/0_NeDongSem/Etc/CSV/Map/" + SceneName + ".csv";
        return CSVMng.Instance.Get_CSVTextLineArray(CSVPath);
    }

    private void CreateStageMap_Init(int _iTileCountY , int _iTileCountX = 0)
    {
        int TileCountX, TileCountY;
        TileCountY = _iTileCountY;
        if (_iTileCountX <= 0)
        {
            TileCountX = TileCountY;
        }
        else
        {
            TileCountX = _iTileCountX;
        }

        int StageMapTileListCount = m_StageMapTileList.Count;

        if(StageMapTileListCount < TileCountY)
        {
            int AddCount = TileCountY - StageMapTileListCount;

            for(int i = 0; i < AddCount; ++i)
            {
                List<GameObject> TileLineList = new List<GameObject>();
                m_StageMapTileList.Add(TileLineList);

                string Name = "TileLine" + (StageMapTileListCount + i + 1).ToString();
                GameObject TileLineGameObject = new GameObject(Name);
                TileLineGameObject.transform.parent = m_CloneTile.transform;
            }
        }

        for(int i = 0; i < TileCountY; ++i)
        {
            int LineTileCount = m_StageMapTileList[i].Count;

            if(LineTileCount < TileCountX)
            {
                int AddCount = TileCountX - LineTileCount;

                for (int j = 0; j < AddCount; ++j)
                {
                    Transform TileLineTransform = m_CloneTile.transform.GetChild(i);
                    GameObject Tile = Instantiate(m_Tile, TileLineTransform);
                    Tile.name += (LineTileCount + j + 1).ToString();
                    m_StageMapTileList[i].Add(Tile);
                }
            }
        }
    }

    private void CreateStageMap_Setting(string[] _CSVTextLineArray, int _iTileCountY, int _iTileCountX = 0)
    {
        int CloneTileChildCount = m_CloneTile.transform.childCount;
        for (int i = 0; i < CloneTileChildCount; ++i)
        {
            Transform TileLineTransform = m_CloneTile.transform.GetChild(i);
            if(i < _iTileCountY)
            {
                TileLineTransform.gameObject.SetActive(true);
                TileLineTransform.localPosition = Vector3.zero;
                TileLineTransform.localRotation = Quaternion.identity;
                TileLineTransform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                TileLineTransform.gameObject.SetActive(false);
            }
        }

        int TileCountX, TileCountY;
        if (_iTileCountX == 0)
        {
            TileCountX = _iTileCountY;
        }
        else
        {
            TileCountX = _iTileCountX;
        }
        TileCountY = _iTileCountY;

        float fCenterPosX = (TileCountX * TileSize.X) * 0.5f;
        float fCenterPosY = (TileCountY * TileSize.Y) * 0.5f;
        float fFirstPosX = 0 - fCenterPosX + (TileSize.X * 0.5f);
        float fFirstPosY = fCenterPosY - (TileSize.Y * 0.5f);

        for (int i = 0; i < _CSVTextLineArray.Length; ++i)
        {
            string[] TextArray = CSVMng.Instance.Get_CSVTextDivision(_CSVTextLineArray[i], ",");

            for (int j = 0; j < m_StageMapTileList[i].Count; ++j)
            {
                if (j < _iTileCountX)
                {
                    Tile_Setting(fFirstPosX + (j * TileSize.X), fFirstPosY - (i * TileSize.Y), m_StageMapTileList[i][j], TextArray[j]);
                }
                else
                {
                    m_StageMapTileList[i][j].SetActive(false);
                }
            }
        }
    }

    private void Tile_Setting(float _fPosX, float _fPosY, GameObject _TileGameObject, string _strTile)
    {
        eTileType TileType = StringToTileType(_strTile);
        if(TileType == eTileType.Enemy)
        {
            EnemyTile_Setting(_TileGameObject, _strTile);
        }
        RectTransform TileRectTransform = _TileGameObject.GetComponent<RectTransform>();
        TileRectTransform.anchoredPosition = new Vector2(_fPosX, _fPosY);
        TileRectTransform.sizeDelta = new Vector2(TileSize.X, TileSize.Y);
        TileRectTransform.Rotate(Vector3.zero);
        TileRectTransform.localScale = new Vector3(1, 1, 1);

        Image TileImage = _TileGameObject.GetComponent<Image>();
        TileImage.sprite = m_TileSpriteList[(int)TileType];
    }

    private eTileType StringToTileType(string _strTile)
    {
        eTileType TileType = eTileType.TileType_End;

        if(_strTile == "")
        {
            TileType = eTileType.NDS;
        }
        else if(_strTile == "0")
        {
            TileType = eTileType.Edge;
        }
        else if(_strTile[0] == '1')
        {
            TileType = eTileType.Enemy;
        }
        else if (_strTile == "2")
        {
            TileType = eTileType.NDS;
        }
        else if(_strTile == "3")
        {
            TileType = eTileType.Block;
        }
        else
        {
            Debug.Log("CSV Map Text Error");
        }
        return TileType;
    }

    private void EnemyTile_Setting(GameObject _EnemyTileGameObject, string _strEnemyTile)
    {
        string EnemyTile = _strEnemyTile;
        EnemyTile = EnemyTile.Substring(2, EnemyTile.Length - 3);
        string[] EnemyTileNum = EnemyTile.Split("&");

        for(int i = 0; i < EnemyTileNum.Length; ++i)
        {
            int iEnemyTileNum = int.Parse(EnemyTileNum[i]);
            m_EnemyTileList[iEnemyTileNum] = _EnemyTileGameObject;
        }
    }
}
