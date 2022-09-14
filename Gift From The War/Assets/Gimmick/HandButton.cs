using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandButton : MonoBehaviour
{
    [SerializeField] private Material onGimmickButtonMat;
    [SerializeField] private Material offGimmickButtonMat;
    [SerializeField] private Material clearGimmickButtonMat;
    [SerializeField] private Material stopGimmickButtonMat;
    [SerializeField] private Light pointLight;
    [SerializeField] Color onGimmcickColor = new Color(1, 1, 1, 1);
    [SerializeField] Color offGimmcikColor = new Color(1, 1, 1, 1);
    [SerializeField] Color clearGimmickColor = new Color(1, 1, 1, 1);
    [SerializeField] Transform subTrans = null;


    public HandGimmick gimmick = null;

    public bool changeFlg { get; set; }
    public bool clearFlg { get; set; }

    void Start()
    {
        changeFlg = false;
        clearFlg = false;

        if (subTrans != null && SaveManager.Instance.nowSaveData.getGunFlg == false)
        {
            transform.position = subTrans.position;
            transform.rotation = subTrans.rotation;
            transform.localScale = subTrans.localScale;
        }
    }

    void Update()
    {
        if (clearFlg == true) return;

        if (gimmick == null)
        {
            this.transform.GetChild(1).GetComponent<MeshRenderer>().material = stopGimmickButtonMat;
            pointLight.color = new Color(0, 0, 0, 0.5f);
        }
        else
        {
            if (changeFlg)
            {
                this.transform.GetChild(1).GetComponent<MeshRenderer>().material = onGimmickButtonMat;
                pointLight.color = onGimmcickColor;
            }
            else
            {
                this.transform.GetChild(1).GetComponent<MeshRenderer>().material = offGimmickButtonMat;
                pointLight.color = offGimmcikColor;
            }

            //‚à‚µ‚à”à‚ªŠJ‚¢‚Ä‚¢‚½‚ç
            if (gimmick.openFlg == true)
            {
                this.transform.GetChild(1).GetComponent<MeshRenderer>().material = clearGimmickButtonMat;
                pointLight.color = clearGimmickColor;
                clearFlg = true;
            }
        }
    }
}
