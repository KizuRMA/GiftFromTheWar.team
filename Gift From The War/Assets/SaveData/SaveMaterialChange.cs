using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMaterialChange : MonoBehaviour
{
    [SerializeField] private Material nowSpotMat;
    [SerializeField] private Material pastSpotMat;
    private SaveManager.SaveSpotNum spotNum;
    private SaveManager.SaveSpotNum nowSpotNum;
    private bool cameFlg = false;   //通ったことがあるか

    void Start()
    {
        spotNum = this.transform.parent.parent.GetChild(0).gameObject.GetComponent<SaveSpotData>().GetSpotNum();
    }

    void Update()
    {
        nowSpotNum = SaveManager.Instance.nowSaveData.saveSpotNum;

        //今ここのセーブスポットにいたら
        if (spotNum == nowSpotNum)
        {
            cameFlg = true;
            this.GetComponent<MeshRenderer>().material = nowSpotMat;
            return;
        }

        if (!cameFlg) return;

        //過去にこのセーブスポットを通っていたら
        this.GetComponent<MeshRenderer>().material = pastSpotMat;
    }
}
