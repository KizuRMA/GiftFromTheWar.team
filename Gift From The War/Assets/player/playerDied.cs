using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDied : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    private CharacterController CC;
    private Transform trans;
    [SerializeField] private Gravity gravity;
    [SerializeField] private GameObject rantan;
    [SerializeField] private GameObject gun;
    private Rigidbody rantanRD;
    private Rigidbody gunRD;

    public bool diedFlg { get; set; }

    //移動
    [SerializeField] private float height;      //高さ
    [SerializeField] private float downSpeed;   //下がるスピード
    [SerializeField] private float downMax;     //下がる最大値
    private float nowGravity;                   //今の重力加速度

    //回転
    [SerializeField] private float rotSpeed;    //回転スピード
    [SerializeField] private float rotMax;      //回転の最大値
    private float rotSum = 0;                   //回転の合計値
    [SerializeField] private float gunRotSpeed; //銃の回転スピード

    private bool groundFlg = false;    //一度でも地面についたかどうか

    void Start()
    {
        CC = this.GetComponent<CharacterController>();
        trans = transform;
        rantanRD = rantan.GetComponent<Rigidbody>();
        gunRD = gun.GetComponent<Rigidbody>();
        diedFlg = false;
        nowGravity = gravity.GetGravity * Time.deltaTime;
    }

    void Update()
    {
        if (CC.GetComponent<playerAbnormalcondition>().life <= 0)
        {
            diedFlg = true;

            CC.height = height;

            //親子関係削除
            rantan.transform.parent = null;
            gun.transform.parent = null;

            rantanRD.useGravity = true;
            gunRD.useGravity = true;

            //移動角度制限削除
            rantanRD.constraints = RigidbodyConstraints.None;
            gunRD.constraints = RigidbodyConstraints.None;
        }

        if (!diedFlg) return;

        DownKnees();

        Fall();
    }

    private void DownKnees()    //膝をつく
    {
        //レイ判定で地面に着いたか確認する
        Ray ray = new Ray(trans.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, downMax))
        {
            groundFlg = true;
        }

        nowGravity += gravity.GetGravity * Time.deltaTime;
        CC.Move(new Vector3(0, nowGravity, 0) * Time.deltaTime);   //プレイヤーを移動

    }

    private void Fall()
    {
        if (!groundFlg) return;
        if (rotSum > rotMax) return;

        rotSum += rotSpeed * Time.deltaTime;    //回転の合計を保存
        trans.rotation *= Quaternion.Euler(rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime);    //プレイヤーを回転
        gun.transform.rotation *= Quaternion.Euler(0, 0, gunRotSpeed * Time.deltaTime); //銃を回転
    }
}
