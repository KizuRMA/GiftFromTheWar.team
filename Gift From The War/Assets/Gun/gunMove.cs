using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunMove : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    [SerializeField] private GameObject player;
    private Transform trans;
    private FPSController fpsC;
    [SerializeField] private playerHundLadder playerHund;
    [SerializeField] private playerDied playerDied;

    //銃の移動
    private Vector3 firstPos;                   //最初の位置
    private int upDown = 1;                     //上がるか下がるかの符号
    private float posY = 0;                     //Y座標の移動量
    [SerializeField] private float upDownSpeed; //上がり下がりの速さ
    [SerializeField] private float maxPosY;     //最大移動位置
    [SerializeField] private float dashRaito;   //走った時の補正倍率

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

    void tremor()   //プレイヤーの動きによる移動
    {
        if (fpsC.moveFlg)  //プレイヤーが動いているか
        {
            Move();
        }
        else
        {
            Return();
        }

        trans.localPosition += new Vector3(0.0f, posY, 0.0f) * Time.deltaTime;
    }

    private void Move() //銃の移動
    {
        if (Mathf.Abs(trans.localPosition.y - firstPos.y) > Mathf.Abs(maxPosY))   //上下の移動のチェンジ
        {
            upDown *= -1;
        }

        if (fpsC.dashFlg)  //プレイヤーが走っているか
        {
            posY = upDownSpeed * upDown * dashRaito;
        }
        else
        {
            posY = upDownSpeed * upDown;
        }
    }

    private void Return()   //銃が戻る
    {
        //自動で戻る処理
        float nowPos = firstPos.y - trans.localPosition.y;
        bool largeMoveFlg = Mathf.Abs(nowPos) > upDownSpeed * Time.deltaTime;   //大きく動く必要があるか
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
