using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVMng : MonoBehaviour
{
    static private CSVMng instance = null;
    static public CSVMng Instance { get { return instance; } }

    private void Awake()
    {
        //InstanceCheck
        if (ReferenceEquals(instance, null))
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public string[] Get_CSVTextLineArray(string _strPath)
    {
        string[] CSVTextLineArray = System.IO.File.ReadAllLines(_strPath);
        return CSVTextLineArray;
    }

    public string[] Get_CSVTextDivision(string _strText, string _strDivisionText)
    {
        return _strText.Split(_strDivisionText);
    }
}
