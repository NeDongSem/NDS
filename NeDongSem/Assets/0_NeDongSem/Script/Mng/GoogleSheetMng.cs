using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleSheetMng : MonoBehaviour
{
    static private GoogleSheetMng instance = null;
    static public GoogleSheetMng Instance { get { return instance; } }

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

    private void Init()
    {

    }
}
