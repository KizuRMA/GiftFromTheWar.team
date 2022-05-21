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
        if (fpsC.GetMoveFlg())  //プレイヤーが動いているか
        {
            if (Mathf.Abs(trans.localPosition.y - firstPos.y) > Mathf.Abs(maxPosY))   //上下の移動のチェンジ
            {
                upDown *= -1;
            }

            if (fpsC.GetDashFlg())  //プレイヤーが走っているか
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

            //自動で戻る処理
            float nowPos = firstPos.y - trans.localPosition.y;
            if (Mathf.Abs(nowPos) > upDownSpeed * Time.deltaTime) //ほぼ最初のところに戻っていなかったら
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
