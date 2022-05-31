using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnetChain : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    private Transform trans;
    [SerializeField] private CharacterController CC;    
    [SerializeField] private Transform playerTrans;
    [SerializeField] private Transform camTrans;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private remainingAmount energyAmount;

    //弾の発射
    [SerializeField] private float shotSpeed;   //発射スピード
    [SerializeField] private float useEnergy;   //消費エネルギー
    private bool shotFlg;                       //発射可能
    private Vector3 shotPos;                    //着弾点

    //移動処理
    public bool metalFlg { get; set; }              //金属にくっついたフラグ
    public bool magnetFlg { get; set; }               //磁石移動中のフラグ
    [SerializeField] private float moveSpeed;       //移動する速さ
    [SerializeField] private float range;   
    private bool moveFinishFlg = false;             //移動が終わったフラグ
    private bool hitFlg = false;                    //オブジェクトにあたったか
    private Vector3 prePos;                         //前フレームの位置を記憶しておく
    private bool useEnergy0;                        //エネルギー消費量を0にするフラグ

    private void Start()
    {
        trans = transform;
        metalFlg = false;
    }

    void Update()
    {
        //エネルギーが必要量あれば
        shotFlg = energyAmount.GetSetNowAmount > (1.0f - useEnergy);

        if(useEnergy0)  //エネルギー消費量を0にする
        {
            useEnergy0 = false;
            energyAmount.GetSetNowAmount = 0;
        }

        //発射キーを押したら
        if (Input.GetMouseButton(0))
        {
            if (!shotFlg) return;
            if (magnetFlg) return;
            Shot();
        }

        //発射した弾が金属に当たってなかったら、処理しない
        if (!metalFlg) return;

        MagnetChain();
    }

    private void Shot() //弾を打つ処理
    {
        energyAmount.GetSetNowAmount = useEnergy;
        energyAmount.useDeltaTime = false;

        BulletVecter();

        CreateBullet();

        useEnergy0 = true;
    }

    private void BulletVecter() //弾の向きを決める
    {
        //プレイヤーの前にレイ判定を飛ばし、オブジェクトとの距離を求める。
        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            shotPos = hit.point;
        }
    }

    private void CreateBullet() //プレハブから弾を作る
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, trans.position, Quaternion.identity);
        trans.LookAt(shotPos);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(trans.forward * shotSpeed);

        //射撃されてから3秒後に銃弾のオブジェクトを破壊する.
        Destroy(bullet, 3.0f);
    }

    private void MagnetChain()   //金属の方に飛ぶ
    {
        RangeCheck();

        if (!magnetFlg) return;

        prePos = playerTrans.position;
        PlayerMove();
        PlayerHitJudge();        

        //解除する処理
        if (moveFinishFlg || hitFlg || !magnetFlg)
        {
            Relieve();
        }
    }

    private void RangeCheck()   //射程内かどうか判定
    {
        if (magnetFlg) return;

        bool rangeJudge = Mathf.Abs((trans.position - shotPos).magnitude) < range;
        if (rangeJudge)
        {
            magnetFlg = true;
        }
        else
        {
            Relieve();
        }
    }

    private void PlayerMove()   //プレイヤーの移動
    {
        Vector3 moveVec = shotPos - trans.position; //移動方向算出

        if (Mathf.Abs(moveVec.magnitude) > moveSpeed * Time.deltaTime)  //移動量が大きすぎたら、一定にする
        {
            moveVec = moveVec.normalized * moveSpeed * Time.deltaTime;
        }
        else
        {
            moveFinishFlg = true;
        }

        CC.Move(moveVec);
    }

    private void PlayerHitJudge()
    {
        float moveAmount = Mathf.Abs(playerTrans.position.magnitude) - Mathf.Abs(prePos.magnitude); //前のフレームからの移動量

        if(Mathf.Abs(moveAmount) <= 0.0001)   //移動量が、小さすぎたら、引っかかってるとみなす
        {
            hitFlg = true;     
        }
    }

    private void Relieve()   //解除処理
    {
        magnetFlg = false;
        metalFlg = false;
        moveFinishFlg = false;
        hitFlg = false;
        return;
    }
}
