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
    private List<GameObject> effectList;

    void Start()
    {
        spotNum = this.transform.parent.parent.GetChild(0).gameObject.GetComponent<SaveSpotData>().GetSpotNum();
        effectList = new List<GameObject>();
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

            //前のエフェクト削除
            if (effectList.Count > 0)
            {
                Destroy(effectList[0]);
                effectList.RemoveAt(0);
            }

            //エフェクト再生
            effectList.Add(Instantiate(saveEffect, pos, Quaternion.identity));
            effectFlg = true;

            return;
        }

        effectFlg = false;

        if (!cameFlg) return;

        //過去にこのセーブスポットを通っていたら
        this.GetComponent<MeshRenderer>().material = pastSpotMat;
    }
}
