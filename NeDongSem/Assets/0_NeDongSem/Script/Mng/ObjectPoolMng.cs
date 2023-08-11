using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMng : MonoBehaviour
{
    static private ObjectPoolMng instance = null;
    static public ObjectPoolMng Instance { get { return instance; } }


    public List<GameObject> m_PoolingObjectFrefabsList = new List<GameObject>();
    public List<string> m_PoolingObjectNameList = new List<string>();
    public int m_BasicCreateCount = 10;

    Dictionary<string, GameObject> m_ObjectSourceDictionary;
    Dictionary<string,Queue<GameObject>> m_ObjectPoolDictionary;

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
        m_ObjectSourceDictionary = new Dictionary<string, GameObject>();
        m_ObjectPoolDictionary = new Dictionary<string, Queue<GameObject>>();

        for(int i = 0; i < m_PoolingObjectFrefabsList.Count; ++i)
        {
            Set_PoolingObject(m_PoolingObjectFrefabsList[i], m_PoolingObjectNameList[i]);
        }
    }

    public void Set_PoolingObject(GameObject _PoolingObjectSource, string _strName)
    {
        //예외 처리
        if(_PoolingObjectSource == null)
        {
            Debug.Log("넘겨받은 GameObject가 널값 - ObjectPoolMng.Set_PoolingObject");
            return;
        }

        if(m_ObjectSourceDictionary.ContainsKey(_strName))
        {
            Debug.Log("넘겨받은 Name 이 이미 있음. 다른걸로 변경 바람 - ObjectPoolMng.Set_PoolingObject");
            return;
        }

        AddPoolingObjectSource(_PoolingObjectSource, _strName);
        Queue<GameObject> ObjectPoolQueue = new Queue<GameObject>();
        GameObject PoolingObject;
        for(int i = 0; i < m_BasicCreateCount; ++i)
        {
            PoolingObject = CreatePoolingObject(_strName);
            ObjectPoolQueue.Enqueue(PoolingObject);
        }
        m_ObjectPoolDictionary.Add(_strName, ObjectPoolQueue);
    }

    private void AddPoolingObjectSource(GameObject _PoolingObjectSource, string _strName)
    {
        m_ObjectSourceDictionary.Add(_strName, _PoolingObjectSource);
        new GameObject(_strName).transform.parent = transform;
    }

    private GameObject CreatePoolingObject(string _strName)
    {
        GameObject PoolingObject;
        PoolingObject = Instantiate(m_ObjectSourceDictionary[_strName],transform.Find(_strName));
        PoolingObject.GetComponent<PoolingObject>()?.Set_Name(_strName);
        PoolingObject.SetActive(false);

        return PoolingObject;
    }

    public GameObject Get_PoolingObject(string _strName)
    {
        GameObject PoolingObject;
        if (m_ObjectPoolDictionary[_strName].Count > 0)
        {
            PoolingObject = m_ObjectPoolDictionary[_strName].Dequeue();
        }
        else
        {
            PoolingObject = CreatePoolingObject(_strName);
        }
        PoolingObject.GetComponent<PoolingObject>()?.Set_Ready();
        return PoolingObject;
    }

    public void Return_PoolingObject(GameObject _PoolingObject, string _strName)
    {
        _PoolingObject.GetComponent<PoolingObject>()?.Set_End();
        _PoolingObject.transform.parent = transform.Find(_strName);
        m_ObjectPoolDictionary[_strName].Enqueue(_PoolingObject);
    }

    public void Delete_ObjectPool(string _strName)
    {
        m_ObjectPoolDictionary[_strName].Clear();
        m_ObjectPoolDictionary.Remove(_strName);
        m_ObjectSourceDictionary.Remove(_strName);
    }
}
