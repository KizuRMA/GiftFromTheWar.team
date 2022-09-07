using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItem : MonoBehaviour
{
    //�I�u�W�F�N�g��X�N���v�g���Ƃ��Ă���
    private GameObject gunObj;
    [SerializeField] private Transform camTrans;
    [SerializeField] private Cylinder cylinder;
    [SerializeField] private Image targetImage;
    [SerializeField] private UIBlinking itemBlinking;
    [SerializeField] private DocumentOpen gunDocument;
    [SerializeField] private DocumentOpen gunAmmDocument;

    private bulletChange bulletChange;

    //���C����
    [SerializeField] private float handDis;
    public bool closeItemFlg { get; set; }
    public bool touchedItemFlg { get; set; }

    //�A�C�e������
    private string tagName;
    private string objName;
    private GameObject hitObj;
    [SerializeField] private GameObject rantanObj;
    [SerializeField] private GameObject keyObj;
    public bool windAmmunitionFlg { get; set; }
    public bool magnetAmmunitionFlg { get; set; }
    public bool fireAmmunitionFlg { get; set; }

    void Start()
    {
        //�ϐ���������
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

        TimeAttackItem();
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

            if (!(tagName == "gun" || tagName == "ammunition" || tagName == "gimmickButton" || tagName == "Rantan" || tagName == "Key")) return;   //�G�����̂��A�C�e���łȂ������珈�����Ȃ�

            closeItemFlg = true;

            //�`���[�g���A���\��
            if (touchedItemFlg == false && (tagName == "gun" || tagName == "ammunition" || tagName == "Rantan"))
            {
                if (itemBlinking != null)
                {
                    itemBlinking.SetActive();
                }

                touchedItemFlg = true;
            }

            if (!Input.GetKey(KeyCode.Space)) return;   //�X�y�[�X������Ȃ�������A�������Ȃ�

            closeItemFlg = false;

            JudgeItem();
        }
    }

    private void JudgeItem()    //�E�����A�C�e�����ɂ�鏈��
    {
        if (tagName == "gun")
        {
            gunObj.SetActive(true);
            SaveManager.Instance.nowSaveData.getGunFlg = true;
            SaveManager.Instance.WriteFile();
            SaveManager.Instance.WriteSubFile();
            targetImage.enabled = true;
            bulletChange.HaveBulletAutoChange();

            //�����Đ�
            AudioManager.Instance.PlaySE("GetGun", isLoop: false);

            gunDocument.Open();

            return;
        }

        if (objName == "WindAmmunition")
        {
            windAmmunitionFlg = true;
            SaveManager.Instance.nowSaveData.getWindFlg = true;
            SaveManager.Instance.WriteFile();
            SaveManager.Instance.WriteSubFile();
            cylinder.windAmmo.SetActive(true);
            bulletChange.HaveBulletAutoChange();
            //�����Đ�
            AudioManager.Instance.PlaySE("GetWindBullet", isLoop: false);

            if (gunAmmDocument != null)
            {
                gunAmmDocument.Open();
            }
            return;
        }

        if (objName == "MagnetAmmunition")
        {
            magnetAmmunitionFlg = true;
            SaveManager.Instance.nowSaveData.getMagnetFlg = true;
            SaveManager.Instance.WriteFile();
            SaveManager.Instance.WriteSubFile();
            cylinder.magnetAmmo.SetActive(true);
            bulletChange.HaveBulletAutoChange();

            if (gunAmmDocument != null)
            {
                gunAmmDocument.Open();
            }
            return;
        }

        if (objName == "FireAmmunition")
        {
            fireAmmunitionFlg = true;
            SaveManager.Instance.nowSaveData.getFireFlg = true;
            SaveManager.Instance.WriteFile();
            SaveManager.Instance.WriteSubFile();
            cylinder.fireAmmo.SetActive(true);
            bulletChange.HaveBulletAutoChange();

            if (gunAmmDocument != null)
            {
                gunAmmDocument.Open();
            }
            return;
        }

        if (tagName == "gimmickButton")
        {
            HandButton handButton = hitObj.transform.GetComponent<HandButton>();
            if (handButton.changeFlg == false)
            {
                handButton.changeFlg = true;
                AudioManager.Instance.PlaySE("HandGimmickSE", hitObj, isLoop: false, vol: 0.2f);
            }
            return;
        }

        if (tagName == "Rantan")
        {
            if (rantanObj == null) return;
            rantanObj.SetActive(true);
            SaveManager.Instance.nowSaveData.getRantanFlg = true;
            SaveManager.Instance.WriteFile();
            SaveManager.Instance.WriteSubFile();
            targetImage.enabled = true;
            AudioManager.Instance.PlaySE("GetRantan", isLoop: false);
            return;
        }

        if (tagName == "Key")
        {
            if (keyObj == null) return;
            keyObj.SetActive(true);
            KeyFinalScript key = hitObj.GetComponent<KeyFinalScript>();

            if (key != null)
            {
                key.isGetKeyFlg = true;
                targetImage.enabled = true;
            }
            return;
        }
    }

    private void TimeAttackItem()
    {
        if (!TimeAttackManager.Instance.timeAttackFlg) return;
        if (!(TimeAttackManager.Instance.nowStage == TimeAttackManager.selectStage.SECOND || TimeAttackManager.Instance.nowStage == TimeAttackManager.selectStage.FINAL)) return;

        gunObj.SetActive(true);
        SaveManager.Instance.nowSaveData.getGunFlg = true;
        targetImage.enabled = true;

        windAmmunitionFlg = true;
        SaveManager.Instance.nowSaveData.getWindFlg = true;
        cylinder.windAmmo.SetActive(true);

        rantanObj.SetActive(true);
        SaveManager.Instance.nowSaveData.getRantanFlg = true;
        targetImage.enabled = true;

        if (TimeAttackManager.Instance.nowStage == TimeAttackManager.selectStage.FINAL)
        {
            magnetAmmunitionFlg = true;
            SaveManager.Instance.nowSaveData.getMagnetFlg = true;
            cylinder.magnetAmmo.SetActive(true);
        }

        bulletChange.HaveBulletAutoChange();
        SaveManager.Instance.WriteFile();
    }
}
