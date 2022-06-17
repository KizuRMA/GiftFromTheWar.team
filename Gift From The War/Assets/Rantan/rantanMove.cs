using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rantanMove : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    [SerializeField] private GameObject player;
    [SerializeField] private Transform camTrans;
    private Transform trans;
    private FPSController fpsC;
    [SerializeField] private playerHundLadder playerHund;
    [SerializeField] private playerDied playerDied;

    //�����^���̈ړ�
    private Vector3 firstPos;                   //�ŏ��̈ʒu
    private int upDown = -1;                    //�オ�邩�����邩�̕���
    private float posY = 0;                     //Y���W�̈ړ���
    [SerializeField] private float upDownSpeed; //�オ�艺����̑���
    [SerializeField] private float maxPosY;     //�ő�ړ��ʒu
    [SerializeField] private float dashRaito;   //���������̕␳�{��

    //�����^���̉�]
    private Quaternion firstQua;                    //�ŏ��̃N�H�[�^�j�I��
    [SerializeField] private float upRaito = 0;     //��̌X���̕␳�{��
    [SerializeField] private float downRaito = 0;   //��̌X���̕␳�{��

    void Start()
    {
        trans = GetComponent<Transform>();
        fpsC = player.GetComponent<FPSController>();
        firstQua = trans.localRotation;
        firstPos = trans.localPosition;
    }

    void Update()
    {
        if (playerDied.diedFlg || playerHund.ClimbLadderFlg()) return;

        rotation();
        tremor();
    }

    void rotation() //�����^���̉�]
    {
        //�J�����̃N�I�[�^�j�I���l���擾
        Quaternion _camQua = camTrans.rotation;

        float _camEulerAngleX = _camQua.eulerAngles.x;

        //�p�x�𒲐�����
        if (_camEulerAngleX >= 300.0f)
        {
            _camEulerAngleX -= 360.0f;
            _camEulerAngleX *= upRaito;
        }
        else
        {
            _camEulerAngleX *= downRaito;
        }

        _camEulerAngleX *= -1;

        //�����^���̉�]����
        Quaternion _ranQua = Quaternion.AngleAxis(_camEulerAngleX, Vector3.right);
        trans.localRotation = _ranQua * firstQua;
    }

    void tremor()   //�ړ��ɂ�郉���^���̗h��
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

    private void Move() //�����^���̈ړ�
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

    private void Return()   //�����^�����߂�
    {
        //�����Ŗ߂鏈��
        float nowPos = firstPos.y - trans.localPosition.y;
        bool largeMoveFlg = Mathf.Abs(nowPos) > upDownSpeed * Time.deltaTime;   //�傫�������K�v�����邩
        if (largeMoveFlg)
        {
            if (nowPos > 0)
            {
                posY = upDownSpeed;
            }
            else
            {
                posY = -upDownSpeed;
            }
        }
        else
        {
            upDown = -1;
            trans.localPosition = firstPos;
            return;
        }
    }
}
