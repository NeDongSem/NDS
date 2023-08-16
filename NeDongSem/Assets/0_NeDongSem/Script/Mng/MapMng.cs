using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public string m_SheetRange = "";

    public int m_TileSizeX;
    public int m_TileSizeY;
    public GameObject m_BackgroundParent;
    public GameObject m_Background;
    public GameObject m_TileParent;
    public GameObject m_Tile;
    public List<Material> m_TileMaterialList;
    List<List<GameObject>> m_StageMapTileList;
    int m_iEnemyTileCount;
    List<GameObject> m_EnemyTileList;
    public List<GameObject> EnemyTileList { get { return m_EnemyTileList; } }
    //struct stTileSize
    //{
    //    int iTileSizeX;
    //    public int X
    //    {
    //        get { return iTileSizeX; }
    //        set { iTileSizeX = value; }
    //    }
    //    int iTileSizeY;
    //    public int Y
    //    {
    //        get { return iTileSizeY; }
    //        set { iTileSizeY = value; }
    //    }
    //}
    //stTileSize TileSize;

    private void Awake()
    {
        //InstanceCheck
        if (ReferenceEquals(instance, null))
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            StartCoroutine("Init");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Init()
    {
        while (ReferenceEquals(GoogleSheetMng.Instance,null))
        {
            yield return new WaitForSeconds(0.1f);
        }

        Set_CreateMap();
    }

    public void Set_CreateMap()
    {
        GoogleSheetMng.delDataProcessingFunc delCreateStageMapFunc = new GoogleSheetMng.delDataProcessingFunc(CreateStageMap);
        GoogleSheetMng.Instance.Get_GoogleSheetData(SceneManager.GetActiveScene().name, m_SheetRange, delCreateStageMapFunc);
    }

    private void CreateStageMap(string[] _strDataLineArray)
    {
        //m_TileList = new List<GameObject>(); ¿ŒΩ∫∆Â≈Õø°º≠ «ÿ¡‹
        m_StageMapTileList = new List<List<GameObject>>();
        m_EnemyTileList = new List<GameObject>();

        //SetTileSize(16, 16);
        CreateBackground(/*TileSize.X, TileSize.Y*/);

        EnemyTile_Init(_strDataLineArray);
        int iTileCountX = CSVMng.Instance.Get_CSVTextDivision(_strDataLineArray[0], ",").Length;
        CreateStageMap_Init(_strDataLineArray.Length, iTileCountX);
        CreateStageMap_Setting(_strDataLineArray, _strDataLineArray.Length, iTileCountX);

        //string[] CSVTextLineArray = Get_NowStageMapCSV();
        //EnemyTile_Init(CSVTextLineArray);
        //int iTileCountX = CSVMng.Instance.Get_CSVTextDivision(CSVTextLineArray[0], ",").Length;
        //CreateStageMap_Init(CSVTextLineArray.Length, iTileCountX);
        //CreateStageMap_Setting(CSVTextLineArray, CSVTextLineArray.Length, iTileCountX);
    }

    //private void SetTileSize(int _iTileSizeX = 0, int _iTileSizeY = 0)
    //{
    //    if (_iTileSizeX != 0)
    //    {
    //        TileSize.X = _iTileSizeX;
    //    }
    //    if (_iTileSizeY != 0)
    //    {
    //        TileSize.Y = _iTileSizeY;
    //    }
    //}
    private void CreateBackground(/*int _iSizeX, int _iSizeY*/)
    {
        Transform BackgroundTransform = Instantiate(m_Background, m_BackgroundParent.transform).transform;
        BackgroundTransform.position = new Vector3(0f, 0f, 0.01f);
        BackgroundTransform.Rotate(Vector3.zero);
        Vector3 BackgroundScale = BackgroundTransform.localScale;
        //BackgroundScale.x *= _iSizeX;
        //BackgroundScale.y *= _iSizeY;
        BackgroundScale.x *= m_TileSizeX;
        BackgroundScale.y *= m_TileSizeY;
        BackgroundTransform.localScale = BackgroundScale;
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
                if ((TileLineArray[j] != "") && (TileLineArray[j][0] == '1'))
                {
                    string EnemyTile = TileLineArray[j];
                    EnemyTile = EnemyTile.Substring(2, EnemyTile.Length - 3);
                    string[] EnemyTileNum = EnemyTile.Split("&");

                    for (int k = 0; k < EnemyTileNum.Length; ++k)
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

    private void CreateStageMap_Init(int _iTileCountY, int _iTileCountX = 0)
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

        if (StageMapTileListCount < TileCountY)
        {
            int AddCount = TileCountY - StageMapTileListCount;

            for (int i = 0; i < AddCount; ++i)
            {
                List<GameObject> TileLineList = new List<GameObject>();
                m_StageMapTileList.Add(TileLineList);

                string Name = "TileLine" + (StageMapTileListCount + i + 1).ToString();
                GameObject TileLineGameObject = new GameObject(Name);
                TileLineGameObject.transform.parent = m_TileParent.transform;
            }
        }

        for (int i = 0; i < TileCountY; ++i)
        {
            int LineTileCount = m_StageMapTileList[i].Count;

            if (LineTileCount < TileCountX)
            {
                int AddCount = TileCountX - LineTileCount;

                for (int j = 0; j < AddCount; ++j)
                {
                    Transform TileLineTransform = m_TileParent.transform.GetChild(i);
                    GameObject Tile = Instantiate(m_Tile, TileLineTransform);
                    Tile.name += (LineTileCount + j + 1).ToString();
                    m_StageMapTileList[i].Add(Tile);
                }
            }
        }
    }

    private void CreateStageMap_Setting(string[] _CSVTextLineArray, int _iTileCountY, int _iTileCountX = 0)
    {
        int CloneTileChildCount = m_TileParent.transform.childCount;
        for (int i = 0; i < CloneTileChildCount; ++i)
        {
            Transform TileLineTransform = m_TileParent.transform.GetChild(i);
            if (i < _iTileCountY)
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

        //float fCenterPosX = (TileCountX * TileSize.X) * 0.5f;
        //float fCenterPosY = (TileCountY * TileSize.Y) * 0.5f;
        //float fFirstPosX = 0 - fCenterPosX + (TileSize.X * 0.5f);
        //float fFirstPosY = fCenterPosY - (TileSize.Y * 0.5f);

        float fCenterPosX = (TileCountX * m_TileSizeX) * 0.5f;
        float fCenterPosY = (TileCountY * m_TileSizeY) * 0.5f;
        float fFirstPosX = 0 - fCenterPosX + (m_TileSizeX * 0.5f);
        float fFirstPosY = fCenterPosY - (m_TileSizeY * 0.5f);

        for (int i = 0; i < _CSVTextLineArray.Length; ++i)
        {
            string[] TextArray = CSVMng.Instance.Get_CSVTextDivision(_CSVTextLineArray[i], ",");

            for (int j = 0; j < m_StageMapTileList[i].Count; ++j)
            {
                if (j < _iTileCountX)
                {
                    //Tile_Setting(fFirstPosX + (j * TileSize.X), fFirstPosY - (i * TileSize.Y), m_StageMapTileList[i][j], TextArray[j]);
                    Tile_Setting(fFirstPosX + (j * m_TileSizeX), fFirstPosY - (i * m_TileSizeY), m_StageMapTileList[i][j], TextArray[j]);
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
        if (TileType == eTileType.Enemy)
        {
            EnemyTile_Setting(_TileGameObject, _strTile);
        }
        Transform TileTransform = _TileGameObject.GetComponent<Transform>();
        TileTransform.position = new Vector3(_fPosX, _fPosY, 0f);
        //TileTransform.localScale = new Vector3(TileSize.X, TileSize.Y, 0f);
        TileTransform.localScale = new Vector3(m_TileSizeX, m_TileSizeY, 0f);
        TileTransform.Rotate(Vector3.zero);

        MeshRenderer TileMeshRenderer = _TileGameObject.GetComponent<MeshRenderer>();
        TileMeshRenderer.material = m_TileMaterialList[(int)TileType];
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
        else if (_strTile[0] == '1')
        {
            TileType = eTileType.Enemy;
        }
        else if (_strTile == "2")
        {
            TileType = eTileType.NDS;
        }
        else if (_strTile == "3")
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

        for (int i = 0; i < EnemyTileNum.Length; ++i)
        {
            int iEnemyTileNum = int.Parse(EnemyTileNum[i]);
            m_EnemyTileList[iEnemyTileNum] = _EnemyTileGameObject;
        }
    }
}
