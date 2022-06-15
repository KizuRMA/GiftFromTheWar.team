using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGunFlg : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        this.gameObject.SetActive(!SaveManager.Instance.nowSaveData.getGunFlg);
    }
}
