using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rantanWallTouch : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    private Transform trans;
    private Rigidbody rd;

    //フラグ
    private bool returnFlg = true;              //戻るか
    public bool returnFinishFlg { get; set; }   //戻り終わったか

    //移動
    private Vector3 firstPos;                   //基準の位置
    [SerializeField] private float firstSpeed;  //最初の速さ
    [SerializeField] private float speedRaito;  //速さの倍率
    private float nowSpeed;                     //今の速さ
    private Vector3 forceVec;                   //移動の向き

    void Start()
    {
        trans = transform;
        rd = this.GetComponent<Rigidbody>();
        firstPos = transform.localPosition;
        returnFinishFlg = true;
    }

    void Update()
    {
        EraseInertia();

        //押し戻す処理
        if (returnFlg)
        {
            Return();
        }
    }

    private void EraseInertia() //慣性を消す
    {
        rd.velocity = Vector3.zero;
        rd.angularVelocity = Vector3.zero;
    }

    private void Return() //戻す処理
    {
        ReturnSpeedUp();

        CalculateForceVec();

        ForceVecCorrection();

        trans.localPosition += forceVec;
    }

    private void ReturnSpeedUp() //戻す速さを速くする
    {
        nowSpeed *= speedRaito;
    }

    private void CalculateForceVec() //forceVecの計算  
    {
        forceVec = firstPos - trans.localPosition;
    }

    private void ForceVecCorrection() //ぶるぶるしないための補正
    {
        bool froceVecSmallFlg = forceVec.magnitude > nowSpeed;  //forceVecが小さすぎないか見る
        if (froceVecSmallFlg)
        {
            forceVec = forceVec.normalized * nowSpeed;
        }
        else
        {
            nowSpeed = firstSpeed;
            returnFlg = false;
            returnFinishFlg = true;
        }
    }

    private void OnCollisionEnter(Collision collison)   //衝突
    {
        returnFlg = false;
        returnFinishFlg = false;
        nowSpeed = firstSpeed;
    }

    private void OnCollisionExit(Collision collison)    //衝突終了
    {
        returnFlg = true;
    }
}
