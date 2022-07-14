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
    [SerializeField] private UIBlinking itemBlinking;

    private bulletChange bulletChange;

    //レイ判定
    [SerializeField] private float handDis;
    public bool closeItemFlg { get; set; }
    public bool touchedItemFlg { get; set; }

    //アイテム判定
    private string tagName;
    private string objName;
    private GameObject hitObj;
    [SerializeField] private GameObject rantanObj;
    public bool windAmmunitionFlg { get; set; }
    public bool magnetAmmunitionFlg { get; set; }
    public bool fireAmmunitionFlg { get; set; }

    void Start()
    {
        //変数を初期化
        GunUseInfo _info = transform.GetComponent<GunUseInfo>();
        gunObj = _info.gunModel;
        bulletChange = _info.bulletChange;

        touchedItemFlg = !(SaveManager.Instance.nowSaveData.saveSpotNum == SaveManager.SaveSpotNum.none);

        gunObj.SetActive(SaveManager.Instance.nowSaveData.getGunFlg);
        rantanObj.SetActive(SaveManager.Instance.nowSaveData.getRantanFlg);
        targetImage.enabled = SaveManager.Instance.nowSaveData.getGunFlg;

        closeItemFlg = false;
        tagName = null;

        windAmmunitionFlg = SaveManager.Instance.nowSaveData.getWindFlg;
        magnetAmmunitionFlg = SaveManager.Instance.nowSaveData.getMagnetFlg;
        fireAmmunitionFlg = SaveManager.Instance.nowSaveData.getFireFlg;

        cylinder.windAmmo.SetActive(windAmmunitionFlg);
        cylinder.magnetAmmo.SetActive(magnetAmmunitionFlg);
        cylinder.fireAmmo.SetActive(fireAmmunitionFlg);
        bulletChange.HaveBulletAutoChange();
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

            if (!(tagName == "gun" || tagName == "ammunition" || tagName == "gimmickButton" || tagName == "Rantan")) return;   //触ったのがアイテムでなかったら処理しない

            closeItemFlg = true;

            //チュートリアル表示
            if (touchedItemFlg == false && (tagName == "gun" || tagName == "ammunition" || tagName == "Rantan"))
            {
                itemBlinking.SetActive();
                touchedItemFlg = true;
            }

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
            bulletChange.HaveBulletAutoChange();
            AudioManager.Instance.PlaySE("GetGun", isLoop: false);

            return;
        }

        if(objName == "WindAmmunition")
        {
            windAmmunitionFlg = true;
            SaveManager.Instance.nowSaveData.getWindFlg = true;
            SaveManager.Instance.WriteFile();
            cylinder.windAmmo.SetActive(true);
            bulletChange.HaveBulletAutoChange();
            return;
        }

        if (objName == "MagnetAmmunition")
        {
            magnetAmmunitionFlg = true;
            SaveManager.Instance.nowSaveData.getMagnetFlg = true;
            SaveManager.Instance.WriteFile();
            cylinder.magnetAmmo.SetActive(true);
            bulletChange.HaveBulletAutoChange();
            return;
        }

        if (objName == "FireAmmunition")
        {
            fireAmmunitionFlg = true;
            SaveManager.Instance.nowSaveData.getFireFlg = true;
            SaveManager.Instance.WriteFile();
            cylinder.fireAmmo.SetActive(true);
            bulletChange.HaveBulletAutoChange();
            return;
        }

        if(tagName == "gimmickButton")
        {
            hitObj.transform.GetComponent<HandButton>().changeFlg = true;
            return;
        }

        if (tagName == "Rantan")
        {
            if (rantanObj == null) return;
            rantanObj.SetActive(true);
            SaveManager.Instance.nowSaveData.getRantanFlg = true;
            SaveManager.Instance.WriteFile();
            targetImage.enabled = true;
            AudioManager.Instance.PlaySE("GetRantan", isLoop: false);
            return;
        }
    }
}
