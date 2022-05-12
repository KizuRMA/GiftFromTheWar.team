using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rantanMove : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Transform trans;
    private FPSController fpsC;
    private Transform camTrans;
    private Quaternion firstQua;
    private int upDown = -1;
    private Vector3 firstPos;
    private float posY = 0;
    [SerializeField] private float upDownSpeed;
    [SerializeField] private float maxPosY;
    [SerializeField] private float dashRaito;
    [SerializeField] private float upRaito = 0; //上の傾きの補正倍率
    [SerializeField] private float downRaito = 0; //上の傾きの補正倍率

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        camTrans = GameObject.Find("Main Camera").GetComponent<Transform>();
        fpsC = player.GetComponent<FPSController>();
        firstQua = trans.localRotation;
        firstPos = trans.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        rotation();
        tremor();
    }

    void rotation()
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

        Quaternion _ranQua = Quaternion.AngleAxis(_camEulerAngleX, Vector3.right);
        trans.localRotation = _ranQua * firstQua;
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
            //自動で戻る処理
            float nowPos = firstPos.y - trans.localPosition.y;
            if (Mathf.Abs(nowPos) > upDownSpeed) //ほぼ最初のところに戻っていなかったら
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
                posY = nowPos;
                upDown = -1;
            }
        }
        trans.localPosition += new Vector3(0.0f, posY, 0.0f);
    }
}
