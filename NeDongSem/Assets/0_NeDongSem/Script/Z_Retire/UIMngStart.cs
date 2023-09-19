using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMngStart : MonoBehaviour
{
    void Start()
    {
        UIMng_Legacy.Instance.LoadUI<UIRound>(UIType.UIRound);
        UIMng_Legacy.Instance.LoadUI<UIGold>(UIType.UIGold);
    }
}
