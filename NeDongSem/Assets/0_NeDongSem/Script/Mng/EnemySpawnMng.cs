using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawnMng : MonoBehaviour
{
    static private EnemySpawnMng instance = null;
    static public EnemySpawnMng Instance { get { return instance; } }

    public string m_SheetRange = "";
    public float m_EnemyZ = 0.02f;

    Dictionary<string, List<string>> m_StageEnemySpawnDictionary;
    List<string> m_EnemySpawnList;

    enum eEnemySpawnState
    {
        Spawn,
        Wait,
        eEnemySpawnState_End
    }
    eEnemySpawnState m_eEnemySpawnState = eEnemySpawnState.eEnemySpawnState_End;

    Vector3 m_v3SpawnPos;
    float m_fWaitTime;
    float m_fTermTime;
    float m_fTime;
    int m_iEnemySpawnListIndex;

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
        m_StageEnemySpawnDictionary = new Dictionary<string, List<string>>();
        m_v3SpawnPos = new Vector3();

        while (ReferenceEquals(GoogleSheetMng.Instance, null))
        {
            yield return new WaitForSeconds(0.1f);
        }

        SetEnemySpawnList();
    }

    private void SetEnemySpawnList()
    {
        GoogleSheetMng.delDataProcessingFunc delEnemySpawnListFunc = new GoogleSheetMng.delDataProcessingFunc(Set_EnemySpawnList);
        GoogleSheetMng.Instance.Get_GoogleSheetData("EnemySpawn", m_SheetRange, delEnemySpawnListFunc);
    }

    public void Set_EnemySpawnList(string[] _CSVTextLineArray)
    {
        for (int i = 0; i < _CSVTextLineArray.Length; ++i)
        {
            string[] EnemySpawnArray = _CSVTextLineArray[i].Split(",");
            List<string> EnemySpawnList = new List<string>();
            string strStageName = EnemySpawnArray[0];

            for(int j = 1; j < EnemySpawnArray.Length; ++j)
            {
                EnemySpawnList.Add(EnemySpawnArray[j]);
            }

            m_StageEnemySpawnDictionary.Add(strStageName, EnemySpawnListSetting(EnemySpawnList));
        }
    }

    //t와 w는 제외하고 에너미만 곱셈인 것 풀어서 저장
    private List<string> EnemySpawnListSetting(List<string> _EnemySpawnList)
    {
        string strEnemy;
        string[] EnemyTypeNumArray;
        int iNum;
        for(int i = 0; i < _EnemySpawnList.Count; ++i)
        {
            if(_EnemySpawnList[i].Contains('*'))
            {
                EnemyTypeNumArray = _EnemySpawnList[i].Split('*');
                strEnemy = EnemyTypeNumArray[0];
                iNum = int.Parse(EnemyTypeNumArray[1]);

                for(int j = 0; j < iNum; ++j)
                {
                    _EnemySpawnList.Insert(i + 1, strEnemy);
                }
                _EnemySpawnList.RemoveAt(i);
            }
        }

        return _EnemySpawnList;
    }

    public void Start_EnemySpawn()
    {
        if (m_StageEnemySpawnDictionary.Count != 0)
        {
            m_EnemySpawnList = m_StageEnemySpawnDictionary[SceneManager.GetActiveScene().name];
            Vector3 v3SpawnPos = MapMng.Instance.EnemyTileList[0].transform.position;
            m_v3SpawnPos.x = v3SpawnPos.x;
            m_v3SpawnPos.y = v3SpawnPos.y;
            m_v3SpawnPos.z = m_EnemyZ;
            m_eEnemySpawnState = eEnemySpawnState.Spawn;
            m_iEnemySpawnListIndex = 0;
            string strFirstTerm = m_EnemySpawnList[m_iEnemySpawnListIndex++];
            strFirstTerm = strFirstTerm.Remove(0, 1);
            m_fTermTime = float.Parse(strFirstTerm);
            m_fTime = m_fTermTime; //시작하자 마자 바로 소환하려고
            m_fWaitTime = 0f;

            StartCoroutine("EnemySpawn_Coroutine");
        }
    }

    private IEnumerator EnemySpawn_Coroutine()
    {
        while(m_eEnemySpawnState != eEnemySpawnState.eEnemySpawnState_End)
        {
            if (m_eEnemySpawnState == eEnemySpawnState.Spawn)
            {
                if (m_fTime >= m_fTermTime)
                {
                    m_fTime = 0f;
                    EnemySpawn();
                    CheckEnemySpawnState();
                }
                else
                {
                    m_fTime += Time.deltaTime;
                }
            }
            else if(m_eEnemySpawnState == eEnemySpawnState.Wait)
            {
                if (m_fTime >= m_fWaitTime)
                {
                    m_fTime = 0f;
                    m_fWaitTime = 0f;
                    CheckEnemySpawnState();
                }
                else
                {
                    m_fTime += Time.deltaTime;
                }
            }
            yield return null;
        }
    }

    private void EnemySpawn()
    {
        GameObject EnemyGameObject = ObjectPoolMng.Instance.Get_PoolingObject(m_EnemySpawnList[m_iEnemySpawnListIndex++]);
        EnemyGameObject.SetActive(true);
        m_v3SpawnPos.z += 0.001f;
        EnemyGameObject.transform.position = m_v3SpawnPos;
    }

    private void CheckEnemySpawnState()
    {
        if (m_EnemySpawnList.Count  == m_iEnemySpawnListIndex)
        {
            m_eEnemySpawnState = eEnemySpawnState.eEnemySpawnState_End;
            return;
        }

        string strEnemySpawn = m_EnemySpawnList[m_iEnemySpawnListIndex];
        if(strEnemySpawn[0] == 't')
        {
            string strFirstTerm = m_EnemySpawnList[m_iEnemySpawnListIndex++];
            strFirstTerm = strFirstTerm.Remove(0, 1);
            m_fTermTime = float.Parse(strFirstTerm);
            CheckEnemySpawnState();
        }
        else if(strEnemySpawn[0] == 'w')
        {
            m_iEnemySpawnListIndex++;
            m_eEnemySpawnState = eEnemySpawnState.Wait;
            m_fWaitTime = float.Parse(strEnemySpawn.Remove(0, 1));
        }
        else
        {
            m_eEnemySpawnState = eEnemySpawnState.Spawn;
        }
    }

    private void Start()
    {
        Invoke("Start_EnemySpawn", 3.0f);
    }
}
