using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rantanMove : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Transform trans;
    private FPSController fpsC;
    [SerializeField] playerHundLadder playerHund;
    [SerializeField] playerDied playerDied;
    [SerializeField] private Transform camTrans;
    private Quaternion firstQua;
    private int upDown = -1;
    private Vector3 firstPos;
    private float posY = 0;
    [SerializeField] private float upDownSpeed;
    [SerializeField] private float maxPosY;
    [SerializeField] private float dashRaito;
    [SerializeField] private float upRaito = 0; //��̌X���̕␳�{��
    [SerializeField] private float downRaito = 0; //��̌X���̕␳�{��

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        fpsC = player.GetComponent<FPSController>();
        firstQua = trans.localRotation;
        firstPos = trans.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDied.diedFlg) return;

        rotation();
        tremor();
    }

    void rotation()
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

        Quaternion _ranQua = Quaternion.AngleAxis(_camEulerAngleX, Vector3.right);
        trans.localRotation = _ranQua * firstQua;
    }

    void tremor()
    {
        if (fpsC.moveFlg)  //�v���C���[�������Ă��邩
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
        else
        {
            if (playerHund.ClimbLadderFlg()) return;

            //�����Ŗ߂鏈��
            float nowPos = firstPos.y - trans.localPosition.y;
            if (Mathf.Abs(nowPos) > upDownSpeed * Time.deltaTime) //�قڍŏ��̂Ƃ���ɖ߂��Ă��Ȃ�������
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

        trans.localPosition += new Vector3(0.0f, posY, 0.0f) * Time.deltaTime;
    }
}
