using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFireFlg : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        this.gameObject.SetActive(!SaveManager.Instance.nowSaveData.getFireFlg);
    }
}
