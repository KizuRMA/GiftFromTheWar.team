using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMaterialChange : MonoBehaviour
{
    [SerializeField] private Material nowSpotMat;
    [SerializeField] private Material pastSpotMat;
    private SaveManager.SaveSpotNum spotNum;
    private SaveManager.SaveSpotNum nowSpotNum;
    private bool cameFlg = false;   //�ʂ������Ƃ����邩

    void Start()
    {
        spotNum = this.transform.parent.parent.GetChild(0).gameObject.GetComponent<SaveSpotData>().GetSpotNum();
    }

    void Update()
    {
        nowSpotNum = SaveManager.Instance.nowSaveData.saveSpotNum;

        //�������̃Z�[�u�X�|�b�g�ɂ�����
        if (spotNum == nowSpotNum)
        {
            cameFlg = true;
            this.GetComponent<MeshRenderer>().material = nowSpotMat;
            return;
        }

        if (!cameFlg) return;

        //�ߋ��ɂ��̃Z�[�u�X�|�b�g��ʂ��Ă�����
        this.GetComponent<MeshRenderer>().material = pastSpotMat;
    }
}
