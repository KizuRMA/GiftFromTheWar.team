using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSpotData : MonoBehaviour
{
    [SerializeField] private SaveManager.SaveSpotNum spotNum;
    private Transform dataSpotTrans;
    [SerializeField] private Transform goalTrans;


    void Start()
    {
        dataSpotTrans = transform;
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        if (SaveManager.Instance.nowSaveData.saveSpotNum == spotNum) return;

        SaveManager.Instance.nowSaveData.saveSpotNum = spotNum;
        SaveManager.Instance.nowSaveData.dataSpotPos = dataSpotTrans.position;
        SaveManager.Instance.nowSaveData.goalPos = goalTrans.position;

        AudioManager.Instance.PlaySE("SaveSpot", isLoop: false, vol: 0.5f);

        SaveManager.Instance.WriteFile();
    }

    public SaveManager.SaveSpotNum GetSpotNum()
    {
        return spotNum;
    }
}
