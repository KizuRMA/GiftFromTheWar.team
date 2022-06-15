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
    private string objName;
    public bool windAmmunitionFlg { get; set; }
    public bool magnetAmmunitionFlg { get; set; }
    public bool fireAmmunitionFlg { get; set; }

    void Start()
    {
        gunObj.SetActive(SaveManager.Instance.nowSaveData.getGunFlg);
        closeItemFlg = false;
        tagName = null;
        windAmmunitionFlg = SaveManager.Instance.nowSaveData.getWindFlg;
        magnetAmmunitionFlg = SaveManager.Instance.nowSaveData.getMagnetFlg;
        fireAmmunitionFlg = SaveManager.Instance.nowSaveData.getFireFlg;
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
            objName = hit.collider.gameObject.name;

            if (!(tagName == "gun" || tagName == "ammunition")) return;   //触ったのがアイテム出なかったら処理しない

            closeItemFlg = true;

            if (!Input.GetKey(KeyCode.Space)) return;   //スペース押されなかったら、処理しない

            closeItemFlg = false;

            hit.collider.gameObject.SetActive(false);

            JudgeItem();
        }
    }

    private void JudgeItem()    //拾ったアイテム毎による処理
    {
        if (tagName == "gun")
        {
            gunObj.SetActive(true);
            SaveManager.Instance.nowSaveData.getGunFlg = true;
            SaveManager.Instance.WriteFile();
            return;
        }

        if(objName == "WindAmmunition")
        {
            windAmmunitionFlg = true;
            SaveManager.Instance.nowSaveData.getWindFlg = true;
            SaveManager.Instance.WriteFile();
            return;
        }

        if (objName == "MagnetAmmunition")
        {
            magnetAmmunitionFlg = true;
            SaveManager.Instance.nowSaveData.getMagnetFlg = true;
            SaveManager.Instance.WriteFile();
            return;
        }

        if (objName == "FireAmmunition")
        {
            fireAmmunitionFlg = true;
            SaveManager.Instance.nowSaveData.getFireFlg = true;
            SaveManager.Instance.WriteFile();
            return;
        }
    }
}
