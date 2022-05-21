using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunMove : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Transform trans;
    private FPSController fpsC;
    [SerializeField] playerHundLadder playerHund;
    private int upDown = 1;
    private Vector3 firstPos;
    private float posY = 0;
    [SerializeField] private float upDownSpeed;
    [SerializeField] private float maxPosY;
    [SerializeField] private float dashRaito;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        fpsC = player.GetComponent<FPSController>();
        firstPos = trans.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        tremor();
    }

    void tremor()
    {
        if (fpsC.GetMoveFlg())  //�v���C���[�������Ă��邩
        {
            if (Mathf.Abs(trans.localPosition.y - firstPos.y) > Mathf.Abs(maxPosY))   //�㉺�̈ړ��̃`�F���W
            {
                upDown *= -1;
            }

            if (fpsC.GetDashFlg())  //�v���C���[�������Ă��邩
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
                upDown = 1;
                trans.localPosition = firstPos;
                return;
            }
        }

        trans.localPosition += new Vector3(0.0f, posY, 0.0f) * Time.deltaTime;
    }
}
