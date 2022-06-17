using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunMove : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    [SerializeField] private GameObject player;
    private Transform trans;
    private FPSController fpsC;
    [SerializeField] private playerHundLadder playerHund;
    [SerializeField] private playerDied playerDied;

    //�e�̈ړ�
    private Vector3 firstPos;                   //�ŏ��̈ʒu
    private int upDown = 1;                     //�オ�邩�����邩�̕���
    private float posY = 0;                     //Y���W�̈ړ���
    [SerializeField] private float upDownSpeed; //�オ�艺����̑���
    [SerializeField] private float maxPosY;     //�ő�ړ��ʒu
    [SerializeField] private float dashRaito;   //���������̕␳�{��

    void Start()
    {
        trans = GetComponent<Transform>();
        fpsC = player.GetComponent<FPSController>();
        firstPos = trans.localPosition;
    }

    void Update()
    {
        if (playerDied.diedFlg || playerHund.ClimbLadderFlg()) return;

        tremor();
    }

    void tremor()   //�v���C���[�̓����ɂ��ړ�
    {
        if (fpsC.moveFlg)  //�v���C���[�������Ă��邩
        {
            Move();
        }
        else
        {
            Return();
        }

        trans.localPosition += new Vector3(0.0f, posY, 0.0f) * Time.deltaTime;
    }

    private void Move() //�e�̈ړ�
    {
        if (Mathf.Abs(trans.localPosition.y - firstPos.y) > Mathf.Abs(maxPosY))   //�㉺�̈ړ��̃`�F���W
        {
            upDown *= -1;
        }

        if (fpsC.dashFlg)  //�v���C���[�������Ă��邩
        {
            posY = upDownSpeed * upDown * dashRaito;
        }
        else
        {
            posY = upDownSpeed * upDown;
        }
    }

    private void Return()   //�e���߂�
    {
        //�����Ŗ߂鏈��
        float nowPos = firstPos.y - trans.localPosition.y;
        bool largeMoveFlg = Mathf.Abs(nowPos) > upDownSpeed * Time.deltaTime;   //�傫�������K�v�����邩
        if (largeMoveFlg)
        {
            posY = nowPos > 0 ? upDownSpeed : -upDownSpeed;
        }
        else
        {
            upDown = 1;
            trans.localPosition = firstPos;
            return;
        }
    }
}
