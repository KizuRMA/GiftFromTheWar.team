using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    //�I�u�W�F�N�g��X�N���v�g���Ƃ��Ă���
    [SerializeField] private GameObject gunObj;
    [SerializeField] private Transform camTrans;

    //���C����
    [SerializeField] private float handDis;
    public bool closeItemFlg { get; set; }

    //�A�C�e������
    private string tagName;
    private string objName;
    public bool windAmmunitionFlg { get; set; }
    public bool magnetAmmunitionFlg { get; set; }
    public bool fireAmmunitionFlg { get; set; }

    void Start()
    {
        gunObj.SetActive(false);
        closeItemFlg = false;
        tagName = null;
        windAmmunitionFlg = false;
        magnetAmmunitionFlg = false;
        fireAmmunitionFlg = false;
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

            if (!(tagName == "gun" || tagName == "ammunition")) return;   //�G�����̂��A�C�e���o�Ȃ������珈�����Ȃ�

            closeItemFlg = true;

            if (!Input.GetKey(KeyCode.Space)) return;   //�X�y�[�X������Ȃ�������A�������Ȃ�

            closeItemFlg = false;

            hit.collider.gameObject.SetActive(false);

            JudgeItem();
        }
    }

    private void JudgeItem()    //�E�����A�C�e�����ɂ�鏈��
    {
        if (tagName == "gun")
        {
            gunObj.SetActive(true);
            return;
        }

        if(objName == "WindAmmunition")
        {
            windAmmunitionFlg = true;
            return;
        }

        if (objName == "MagnetAmmunition")
        {
            magnetAmmunitionFlg = true;
            return;
        }

        if (objName == "FireAmmunition")
        {
            fireAmmunitionFlg = true;
            return;
        }
    }
}
