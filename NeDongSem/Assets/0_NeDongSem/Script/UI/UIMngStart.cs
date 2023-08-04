using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMngStart : MonoBehaviour
{
    void Start()
    {
        UIMng.Instance.LoadUI<UIRound>(UIType.UIRound);
        UIMng.Instance.LoadUI<UIGold>(UIType.UIGold);
    }
}
