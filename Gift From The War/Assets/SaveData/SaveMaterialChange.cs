using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMaterialChange : MonoBehaviour
{
    [SerializeField] private Material nowSpotMat;
    [SerializeField] private Material pastSpotMat;
    [SerializeField] private GameObject saveEffect;
    [SerializeField] private float hight;
    private SaveManager.SaveSpotNum spotNum;
    private SaveManager.SaveSpotNum nowSpotNum;
    private bool cameFlg = false;   //通ったことがあるか
    private bool effectFlg = false;

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

            if (effectFlg) return;
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + hight, transform.position.z);
            Instantiate(saveEffect, pos, Quaternion.identity);
            effectFlg = true;

            return;
        }

        effectFlg = false;

        if (!cameFlg) return;

        //過去にこのセーブスポットを通っていたら
        this.GetComponent<MeshRenderer>().material = pastSpotMat;
    }
}
