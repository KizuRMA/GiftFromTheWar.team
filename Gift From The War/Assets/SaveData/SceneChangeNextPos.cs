using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeNextPos : MonoBehaviour
{
    [SerializeField] private SaveManager.SaveSpotNum spotNum;
    [SerializeField] private Transform goalTrans;

    void Start()
    {
        
    }

    void Update()
    {
        if (!(SaveManager.Instance.nowSaveData.saveSpotNum <= spotNum)) return;
        if (SaveManager.Instance.nowSaveData.goalPos == goalTrans.position) return;

        SaveManager.Instance.nowSaveData.goalPos = goalTrans.position;
    }
}
