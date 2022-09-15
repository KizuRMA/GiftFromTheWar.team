using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class cameraMove : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    private Transform trans;
    [SerializeField] private FPSController fpsC;

    //カメラの移動
    [SerializeField] private Vector3 firstPos;  //基準の位置
    private float posY = 0;                     //Y座標の移動量
    private int upDown = 1;                     //上がるか下がるかの符号を表す
    [SerializeField] private float upDownSpeed; //カメラの移動スピード
    [SerializeField] private float maxPosY;     //カメラの最大移動位置
    [SerializeField] private float dashRaito;   //走った時の倍率

    //頭下げる処理
    [SerializeField] private float downDis;
    private bool downFlg;

    void Start()
    {
        downFlg = false;
        trans = transform;
    }

    void Update()
    {
        if (downFlg == true) return;

        if (fpsC.moveFlg)
        {
            Move();
        }
        else
        {
            Return();
        }

        trans.localPosition = firstPos + new Vector3(0.0f, posY, 0.0f);
    }

    private void Move() //カメラの上下移動
    {
        //走っているか
        if (fpsC.dashFlg)
        {
            posY += upDownSpeed * upDown * dashRaito * Time.deltaTime;
        }
        else
        {
            posY += upDownSpeed * upDown * Time.deltaTime;
        }

        //最大値まで移動したら、向きを逆にする
        if (posY > maxPosY)
        {
            upDown = -1;
        }

        if (posY < -maxPosY)
        {
            upDown = 1;
        }
    }

    private void Return()   //カメラが所定の位置に戻る
    {
        //if (downFlg == true) return;

        bool returnFlg = Mathf.Abs(posY) > upDownSpeed * Time.deltaTime;    //所定の位置に戻る必要があるか
        if (returnFlg)
        {
            //所定の位置に戻る
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
            //ブレを防止する
            posY = 0;
            upDown = 1;
        }
    }

    public void Getup()
    {
        downFlg = false;
    }


    public void Crouch()
    {
        if (downFlg == true) return;
        downFlg = true;
        trans.DOKill();
        trans.DOLocalMoveY(-downDis, 1.2f);
    }

    public void StandUp()
    {
        if (downFlg == false) return;
        trans.DOKill();
        trans.DOLocalMoveY(0.4f, 0.7f).OnComplete(() => downFlg = false);
    }
}
