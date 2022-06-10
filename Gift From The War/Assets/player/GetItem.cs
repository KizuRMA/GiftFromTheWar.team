using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    //オブジェクトやスクリプトをとってくる
    [SerializeField] private GameObject gunObj;
    [SerializeField] private Transform camTrans;

    //レイ判定
    [SerializeField] private float handDis;
    public bool closeItemFlg { get; set; }

    //アイテム判定
    private string tagName;

    void Start()
    {
        gunObj.SetActive(false);
        closeItemFlg = false;
        tagName = null;
    }

    void Update()
    {
        closeItemFlg = false;

        Ray();
    }

    private void Ray()
    {
        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, handDis))
        {
            tagName = hit.collider.gameObject.tag;

            if (!(tagName == "gun")) return;   //触ったのがアイテム出なかったら処理しない

            closeItemFlg = true;

            if (!Input.GetKey(KeyCode.Space)) return;   //スペース押されなかったら、処理しない

            closeItemFlg = false;

            hit.collider.gameObject.SetActive(false);

            JudgeItem();
        }
    }

    private void JudgeItem()
    {
        if (tagName == "gun")
        {
            gunObj.SetActive(true);
        }
    }
}
