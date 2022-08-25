using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandButton : MonoBehaviour
{
    [SerializeField] private Material onGimmickButtonMat;
    [SerializeField] private Material offGimmickButtonMat;
    [SerializeField] private Material stopGimmickButtonMat;

    public HandGimmick gimmick = null;

    public bool changeFlg { get; set; }

    void Start()
    {
        changeFlg = false;
    }

    void Update()
    {
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
        }
    }
}
