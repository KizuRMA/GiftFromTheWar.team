using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTalk : MonoBehaviour
{
    [SerializeField] private Transform camTrans;
    //  ���C����
    [SerializeField] private float handDis;

    //  ���C���Փ˂��Ă��邩�ǂ���
    public bool hitFlg;

    //  �Փ˂����I�u�W�F�N�g�̏���ێ����Ă���
    private string tagName;
    private GameObject hitObj;

    // ��b����
    private GameObject talkPartner;

    [SerializeField] private GameObject talkIcon;
    [SerializeField] private GameObject talkUI;

    private bool talkFlg = false;

    public bool iconFlg=false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray();
        StartTalk();
        //EndTalk();
    }

    private void Ray()
    {
        if (iconFlg) return;

        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;

        //  ���C����
        if (Physics.Raycast(ray, out hit, handDis))
        {
            //  �Փ˂��Ă���I�u�W�F�N�g�̊e�����擾
            tagName = hit.collider.gameObject.tag;
            hitObj = hit.collider.gameObject;

            //  �Փ˂��Ă���I�u�W�F�N�g���l�W�N��������
            if (tagName == "Nezi")
            {
                // �I�u�W�F�N�g����b����Ƃ��Đݒ肷��
                SetTalkPartner(hitObj);
                
                // �X�y�[�X�L�[��������Ȃ������珈�����Ȃ�
                if (!Input.GetKeyDown(KeyCode.Space)) return;
                talkFlg = true;
            }
        }
        else
        {
            talkIcon.SetActive(false);
        }
    }

    // ��b������Z�b�g����
    public void SetTalkPartner(GameObject partnerObj)
    {
        talkIcon.SetActive(true);
        talkPartner = partnerObj;
    }

    public void StartTalk()
    {
        if (talkFlg)
        {
            iconFlg = true;

            talkUI.SetActive(true);
            talkFlg = false;
            talkIcon.SetActive(false);
        }
    }

    //  ���͂�ǂݏI�������R�}���h��ʂ��I��������
    public void EndTalk()
    {
        //if (!ScenarioManager.Instance.endFlg) return;

        talkUI.SetActive(false);
        iconFlg = false;
    }

}
