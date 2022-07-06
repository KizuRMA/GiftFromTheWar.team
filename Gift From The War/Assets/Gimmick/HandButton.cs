using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandButton : MonoBehaviour
{
    [SerializeField] private Material gimmickButtonMat;
    public bool changeFlg { get; set; }

    void Start()
    {
        changeFlg = false;
    }

    void Update()
    {
        if (!changeFlg) return;
        this.transform.GetChild(1).GetComponent<MeshRenderer>().material = gimmickButtonMat;
    }
}
