using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rantanLadder : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    [SerializeField] private CharacterController playerCC;
    [SerializeField] private playerHundLadder playerHund;
    [SerializeField] private rantanWallTouch rantanWallTouch;
    [SerializeField] private gunWallTouch gunWallTouch;
    private Transform trans;

    //梯子を上る
    private Vector3 firstPos;                   //基準の位置
    [SerializeField] private float upDownSpeed; //ランタンの移動スピード
    [SerializeField] private float maxPosY;     //ランタンの最大移動位置

    private bool limitFlg = false;   //最大移動位置に到達するか
    private bool returnLimitFlg = true;   //最初の位置に到達するか

    void Start()
    {
        trans = transform;
        firstPos = trans.localPosition;
    }

    void Update()
    {
        if (playerHund.ClimbLadderFlg())
        {
            TouchLadder();
            return;
        }

        NoTouchLadder();
    }

    private void TouchLadder()
    {
        if (limitFlg) return;  //最大移動位置に到達していたら

        //壁ずりが終わっていたら
        if (!rantanWallTouch.returnFinishFlg) return;
        if (!gunWallTouch.returnFinishFlg) return;

        returnLimitFlg = false;

        bool maxPosFlg = trans.localPosition.y < firstPos.y - maxPosY; //最大移動位置に到達たら
        if (maxPosFlg)
        {
            limitFlg = true;
            return;
        }

        trans.localPosition += new Vector3(0, -upDownSpeed, 0) * Time.deltaTime;    //移動する
    }

    private void NoTouchLadder()
    {
        if (returnLimitFlg) return;   //元の位置に戻っていたら

        limitFlg = false;

        bool returnFlg = trans.localPosition.y > firstPos.y;    //元の位置に戻っていたら
        if (returnFlg)
        {
            returnLimitFlg = true;
            return;
        }

        trans.localPosition += new Vector3(0, upDownSpeed, 0) * Time.deltaTime; //戻る
    }
}
