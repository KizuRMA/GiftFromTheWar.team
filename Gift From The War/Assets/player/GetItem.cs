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

    //���C����
    [SerializeField] private float handDis;
    public bool closeItemFlg { get; set; }

    //�A�C�e������
    private string tagName;
    private string objName;
    private GameObject hitObj;
    public bool windAmmunitionFlg { get; set; }
    public bool magnetAmmunitionFlg { get; set; }
    public bool fireAmmunitionFlg { get; set; }

    void Start()
    {
        //�ϐ���������
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

            if (!(tagName == "gun" || tagName == "ammunition" || tagName == "gimmickButton")) return;   //�G�����̂��A�C�e���łȂ������珈�����Ȃ�

            closeItemFlg = true;

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
