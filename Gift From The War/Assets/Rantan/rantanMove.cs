using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rantanMove : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    [SerializeField] private GameObject player;
    [SerializeField] private Transform camTrans;
    private Transform trans;
    private FPSController fpsC;
    [SerializeField] private playerHundLadder playerHund;
    [SerializeField] private playerDied playerDied;

    //ランタンの移動
    private Vector3 firstPos;                   //最初の位置
    private int upDown = -1;                    //上がるか下がるかの符号
    private float posY = 0;                     //Y座標の移動量
    [SerializeField] private float upDownSpeed; //上がり下がりの速さ
    [SerializeField] private float maxPosY;     //最大移動位置
    [SerializeField] private float dashRaito;   //走った時の補正倍率

    //ランタンの回転
    private Quaternion firstQua;                    //最初のクォータニオン
    [SerializeField] private float upRaito = 0;     //上の傾きの補正倍率
    [SerializeField] private float downRaito = 0;   //上の傾きの補正倍率

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

    void rotation() //ランタンの回転
    {
        //カメラのクオータニオン値を取得
        Quaternion _camQua = camTrans.rotation;

        float _camEulerAngleX = _camQua.eulerAngles.x;

        //角度を調整する
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

        //ランタンの回転を代入
        Quaternion _ranQua = Quaternion.AngleAxis(_camEulerAngleX, Vector3.right);
        trans.localRotation = _ranQua * firstQua;
    }

    void tremor()   //移動によるランタンの揺れ
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

    private void Move() //ランタンの移動
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

    private void Return()   //ランタンが戻る
    {
        //自動で戻る処理
        float nowPos = firstPos.y - trans.localPosition.y;
        bool largeMoveFlg = Mathf.Abs(nowPos) > upDownSpeed * Time.deltaTime;   //大きく動く必要があるか
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
