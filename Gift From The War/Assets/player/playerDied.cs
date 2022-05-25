using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDied : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    private CharacterController CC;
    private Transform trans;
    [SerializeField] private GameObject rantan;
    [SerializeField] private GameObject gun;
    private Rigidbody rantanRD;
    private Rigidbody gunRD;

    public bool diedFlg { get; set; }

    //移動
    [SerializeField] private float downSpeed;   //下がるスピード
    [SerializeField] private float downMax;     //下がる最大値
    private float downSum = 0;                  //ダウンした合計値

    //回転
    [SerializeField] private float rotSpeed;    //回転スピード
    [SerializeField] private float rotMax;      //回転の最大値
    private float rotSum = 0;                   //回転の合計値
    [SerializeField] private float gunRotSpeed; //銃の回転スピード

    void Start()
    {
        CC = this.GetComponent<CharacterController>();
        trans = transform;
        rantanRD = rantan.GetComponent<Rigidbody>();
        gunRD = gun.GetComponent<Rigidbody>();
        diedFlg = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            diedFlg = true;

            CC.enabled = false; //プレイヤーの当たり判定削除

            //親子関係削除
            rantan.transform.parent = null;
            gun.transform.parent = null;

            //移動角度制限削除
            rantan.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            gun.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        if (!diedFlg) return;

        EraseInertia();

        DownKnees();

        Fall();
    }

    private void EraseInertia() //慣性を消す
    {
        rantanRD.velocity = Vector3.zero;
        rantanRD.angularVelocity = Vector3.zero;
        gunRD.velocity = Vector3.zero;
        gunRD.angularVelocity = Vector3.zero;
    }

    private void DownKnees()    //膝をつく
    {
        if (downSum >= downMax) return;

        downSum += downSpeed * Time.deltaTime;  //下がる合計を保存
        trans.position += new Vector3(0, -downSpeed, 0) * Time.deltaTime;   //プレイヤーを移動

        //オブジェクトを自由落下
        rantan.transform.position += new Vector3(0, -downSpeed, 0) * Time.deltaTime;
        gun.transform.position += new Vector3(0, -downSpeed, 0) * Time.deltaTime;
    }

    private void Fall()
    {
        if (downSum < downMax) return;

        if (rotSum >= rotMax) return;

        rotSum += rotSpeed * Time.deltaTime;    //回転の合計を保存
        trans.rotation *= Quaternion.Euler(rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime);    //プレイヤーを回転
        gun.transform.rotation *= Quaternion.Euler(0, 0, gunRotSpeed * Time.deltaTime); //銃を回転
    }
}
