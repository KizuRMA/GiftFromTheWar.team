using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    private Transform trans;
    [SerializeField] private FPSController fpsC;

    //�J�����̈ړ�
    [SerializeField] private Vector3 firstPos;  //��̈ʒu
    private float posY = 0;                     //Y���W�̈ړ���
    private int upDown = 1;                     //�オ�邩�����邩�̕�����\��
    [SerializeField] private float upDownSpeed; //�J�����̈ړ��X�s�[�h
    [SerializeField] private float maxPosY;     //�J�����̍ő�ړ��ʒu
    [SerializeField] private float dashRaito;   //���������̔{��

    //�������鏈��
    [SerializeField] private float downDis;
    private bool downFlg;

    void Start()
    {
        downFlg = true;
        trans = transform;
    }

    void Update()
    {
        if (fpsC.moveFlg)
        {
            Move();
        }
        else
        {
            Return();
        }

        trans.localPosition = firstPos + new Vector3(0.0f, posY, 0.0f);

        WalkDown();
    }

    private void Move() //�J�����̏㉺�ړ�
    {
        //�����Ă��邩
        if (fpsC.dashFlg)
        {
            posY += upDownSpeed * upDown * dashRaito * Time.deltaTime;
        }
        else
        {
            posY += upDownSpeed * upDown * Time.deltaTime;
        }

        //�ő�l�܂ňړ�������A�������t�ɂ���
        if (posY > maxPosY)
        {
            upDown = -1;
        }

        if (posY < -maxPosY)
        {
            upDown = 1;
        }
    }

    private void Return()   //�J����������̈ʒu�ɖ߂�
    {
        bool returnFlg = Mathf.Abs(posY) > upDownSpeed * Time.deltaTime;    //����̈ʒu�ɖ߂�K�v�����邩
        if (returnFlg)
        {
            //����̈ʒu�ɖ߂�
            if (posY > 0)
            {
                posY += -upDownSpeed * Time.deltaTime;
            }
            else
            {
                posY += upDownSpeed * Time.deltaTime;
            }
        }
        else
        {
            //�u����h�~����
            posY = 0;
            upDown = 1;
        }
    }

    private void WalkDown() //�����ł��Ⴊ�ޏ���
    {
        if(!fpsC.dashFlg)
        {
            trans.localPosition += new Vector3(0, -downDis, 0);
        }
    }
}
