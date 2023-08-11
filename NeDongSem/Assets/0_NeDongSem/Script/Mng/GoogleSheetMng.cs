using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetMng : MonoBehaviour
{
    static private GoogleSheetMng instance = null;
    static public GoogleSheetMng Instance { get { return instance; } }

    string m_strSheetID = "1eVBAKkY-TaWC46eZYenJuKrrZvgcRhU5TUOLbkVdMOU";
    public delegate void delDataProcessingFunc(string[] _strDataLineArray);

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

    }

    public void Get_GoogleSheetData(string _strSheetName, string _strRange, delDataProcessingFunc _delDataProcessingFunc)
    {
        string strURL = string.Format("https://docs.google.com/spreadsheets/d/{0}/gviz/tq?tqx=out:csv&sheet={1}&range={2}", m_strSheetID, _strSheetName, _strRange);
        StartCoroutine(GetDataProcessing(strURL, _delDataProcessingFunc));
    }

    IEnumerator GetDataProcessing(string _strURL , delDataProcessingFunc _delDataProcessingFunc)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(_strURL);
        //Ÿ�� �Ŵ������� �ڷ�ƾ���� �ð� ��� �Լ� �߰��� �ڸ� (���� �ð� ���� ������ üũ�ϱ� ����)
        yield return unityWebRequest.SendWebRequest();
        string strGetData = unityWebRequest.downloadHandler.text;
        string[] strDataLineArray = strGetData.Split('\n');

        for (int i = 0; i < strDataLineArray.Length; ++i)
        {
            strDataLineArray[i] = strDataLineArray[i].Replace("\"","");
        }
        _delDataProcessingFunc(strDataLineArray);
    }
}