using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compass : MonoBehaviour
{
    private Transform trans;

    void Start()
    {
        trans = transform;
        Debug.Log(SaveManager.Instance.nowSaveData.saveSpotNum);
    }

    void Update()
    {
        Vector3 compassVec = SaveManager.Instance.nowSaveData.goalPos;
        compassVec.y = trans.position.y;
        trans.LookAt(compassVec);
        trans.localEulerAngles += new Vector3(-90, 180, 0);
    }
}
