using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItem : MonoBehaviour
{
    //オブジェクトやスクリプトをとってくる
    private GameObject gunObj;
    [SerializeField] private Transform camTrans;
    [SerializeField] private Cylinder cylinder;
    [SerializeField] private Image targetImage;

    //レイ判定
    [SerializeField] private float handDis;
    public bool closeItemFlg { get; set; }

    //アイテム判定
    private string tagName;
    private string objName;
    private GameObject hitObj;
    public bool windAmmunitionFlg { get; set; }
    public bool magnetAmmunitionFlg { get; set; }
    public bool fireAmmunitionFlg { get; set; }

    void Start()
    {
        //変数を初期化
        GunUseInfo _info = transform.GetComponent<GunUseInfo>();
        gunObj = _info.gunModel;

        gunObj.SetActive(SaveManager.Instance.nowSaveData.getGunFlg);
        targetImage.enabled = SaveManager.Instance.nowSaveData.getGunFlg;

        closeItemFlg = false;
        tagName = null;

        windAmmunitionFlg = SaveManager.Instance.nowSaveData.getWindFlg;
        magnetAmmunitionFlg = SaveManager.Instance.nowSaveData.getMagnetFlg;
        fireAmmunitionFlg = SaveManager.Instance.nowSaveData.getFireFlg;

        cylinder.windAmmo.SetActive(windAmmunitionFlg);
        cylinder.magnetAmmo.SetActive(magnetAmmunitionFlg);
        cylinder.fireAmmo.SetActive(fireAmmunitionFlg);
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
            hitObj = hit.collider.gameObject;

            if (!(tagName == "gun" || tagName == "ammunition" || tagName == "gimmickButton")) return;   //触ったのがアイテムでなかったら処理しない

            closeItemFlg = true;

            if (!Input.GetKey(KeyCode.Space)) return;   //スペース押されなかったら、処理しない

            closeItemFlg = false;

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
            targetImage.enabled = true;
            return;
        }

        if(objName == "WindAmmunition")
        {
            windAmmunitionFlg = true;
            SaveManager.Instance.nowSaveData.getWindFlg = true;
            SaveManager.Instance.WriteFile();
            cylinder.windAmmo.SetActive(true);
            return;
        }

        if (objName == "MagnetAmmunition")
        {
            magnetAmmunitionFlg = true;
            SaveManager.Instance.nowSaveData.getMagnetFlg = true;
            SaveManager.Instance.WriteFile();
            cylinder.magnetAmmo.SetActive(true);
            return;
        }

        if (objName == "FireAmmunition")
        {
            fireAmmunitionFlg = true;
            SaveManager.Instance.nowSaveData.getFireFlg = true;
            SaveManager.Instance.WriteFile();
            cylinder.fireAmmo.SetActive(true);
            return;
        }

        if(tagName == "gimmickButton")
        {
            hitObj.transform.GetComponent<HandButton>().changeFlg = true;
            return;
        }
    }
}
