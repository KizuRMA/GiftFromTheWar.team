using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkCollision : MonoBehaviour
{
    [SerializeField] private Transform camTrans;
    //  ���C����
    [SerializeField] private float handDis;

    //  ���C���Փ˂��Ă��邩�ǂ���
    private bool hitFlg;

    //  �Փ˂����I�u�W�F�N�g�̏���ێ����Ă���
    private GameObject hitObj;

    PlayerTalk playerTalk;

    // Start is called before the first frame update
    void Start()
    {
        playerTalk = GetComponent<PlayerTalk>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray();
        
        if(hitFlg)
        {
            playerTalk.StartTalk();
            hitFlg = false;
        }
    }

    private void Ray()
    {
        if (playerTalk.talkFlg) return;

        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;

        //  ���C����
        if (Physics.Raycast(ray, out hit, handDis))
        {
            //  �Փ˂��Ă���I�u�W�F�N�g�̊e�����擾
            hitObj = hit.collider.gameObject;

            //  �Փ˂��Ă���I�u�W�F�N�g���l�W�N��������
            if (hitObj.CompareTag("Nezi"))
            {
                playerTalk.OnActiceIcon();
                playerTalk.SetTalkPartner(hitObj);

                // �X�y�[�X�L�[��������Ȃ������珈�����Ȃ�
                if (!Input.GetKeyDown(KeyCode.Space)) return;
                hitFlg = true;
            }
        }
        else
        {
            playerTalk.OffActiceIcon();
        }
    }
}
