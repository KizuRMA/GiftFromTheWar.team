using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRantanFlg : MonoBehaviour
{
    void Update()
    {
        this.gameObject.SetActive(!SaveManager.Instance.nowSaveData.getRantanFlg);
    }
}
