using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandButton : MonoBehaviour
{
    [SerializeField] private Material onGimmickButtonMat;
    [SerializeField] private Material offGimmickButtonMat;
    [SerializeField] private Material clearGimmickButtonMat;
    [SerializeField] private Material stopGimmickButtonMat;

    public HandGimmick gimmick = null;

    public bool changeFlg { get; set; }
    public bool clearFlg { get; set; }

    void Start()
    {
        changeFlg = false;
        clearFlg = false;
    }

    void Update()
    {
        if (clearFlg == true) return;

        if (gimmick == null)
        {
            this.transform.GetChild(1).GetComponent<MeshRenderer>().material = stopGimmickButtonMat;
        }
        else
        {
            if (changeFlg)
            {
                this.transform.GetChild(1).GetComponent<MeshRenderer>().material = onGimmickButtonMat;
            }
            else
            {
                this.transform.GetChild(1).GetComponent<MeshRenderer>().material = offGimmickButtonMat;
            }

            //‚à‚µ‚à”à‚ªŠJ‚¢‚Ä‚¢‚½‚ç
            if (gimmick.openFlg == true)
            {
                this.transform.GetChild(1).GetComponent<MeshRenderer>().material = clearGimmickButtonMat;
                clearFlg = true;
            }
        }
    }
}
