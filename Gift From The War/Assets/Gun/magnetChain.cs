using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnetChain : ShootParent
{
    //ゲームオブジェクトやスクリプト
    [SerializeField] private Transform playerTrans;
    [SerializeField] private GameObject bulletLineEffect;
    private GameObject bulletLinePos;
    private bulletChange bulletChange;
    private magnet magnet;

    //プレイヤー関係のパラメータ
    private CharacterController CC;
    private GetItem getItem;
    private Gravity gravity;

    //弾の発射
    private bool shotFlg;                       //発射可能

    //移動処理
    public bool metalFlg { get; set; }              //金属にくっついたフラグ
    [SerializeField] private float moveSpeed;       //移動する速さ
    [SerializeField] private float moveSpeedMax;    //移動する速さの最大値
    private float nowMoveSpeed;                     //今の移動スピード
    private Vector3 moveVec;                        //移動方向
    private bool moveFinishFlg = false;             //移動が終わったフラグ
    private bool hitFlg = false;                    //オブジェクトにあたったか
    private Vector3 prePos;                         //前フレームの位置を記憶しておく
    private bool useEnergy0 = false;                //エネルギー消費量を0にするフラグ
    [SerializeField] private float hitRange;        //当たり判定の範囲
    private float nowTime = 0;                      //今の移動時間
    [SerializeField] private float timeMax;         //移動時間の限界値

    private void Start()
    {
        CC = playerTrans.GetComponent<CharacterController>();
        gravity = playerTrans.GetComponent<Gravity>();
        getItem = playerTrans.GetComponent<GetItem>();

         //変数初期化
         bulletLinePos = transform.Find("muzzlePosLine").gameObject;

        if (transform.parent != null)
        {
            bulletChange = transform.parent.GetComponent<bulletChange>();
        }

        magnet = transform.GetComponent<magnet>();

        trans = transform;
        metalFlg = false;
        bulletLineEffect.SetActive(false);
    }

    public void Finish()    //武器を切り替えた時の終了処理
    {
        bulletLineEffect.SetActive(false);
        shotFlg = false;
        metalFlg = false;
        moveFinishFlg = false;
        hitFlg = false;
        useEnergy0 = false;
    }

    void Update()
    {
        if (!getItem.magnetAmmunitionFlg) return;   //弾を拾ってなかったら処理しない

        if (magnet.metal != null) return;   //すでに別の磁石を打っていたら処理しない

        MoveBullet();

        if (bulletChange.nowBulletType != bulletChange.bulletType.e_magnet) return; //今の弾の種類が対応してなかったら

        //エネルギーが必要量あれば
        shotFlg = energyAmount.GetSetNowAmount > (1.0f - useEnergy);

        if (useEnergy0)  //エネルギー消費量を0にする
        {
            useEnergy0 = false;
            energyAmount.GetSetNowAmount = 0;
        }

        //発射キーを押したら
        if (Input.GetMouseButtonDown(0))
        {
            if (!shotFlg) return;
            if (metalFlg) return;
            Shot();
        }

        //発射した弾が金属に当たってなかったら、処理しない
        if (!metalFlg)
        {
            bulletLineEffect.SetActive(false);
            return;
        }

        MagnetChain();
    }

    private void Shot() //弾を打つ処理
    {
        energyAmount.GetSetNowAmount = useEnergy;
        energyAmount.useDeltaTime = false;

        nowTime = 0;

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

    private void MagnetChain()   //金属の方に飛ぶ
    {
        prePos = playerTrans.position;
        PlayerMove();
        PlayerHitJudge();

        //時間の処理
        nowTime += Time.deltaTime;

        //解除する処理
        if (moveFinishFlg || hitFlg || nowTime > timeMax)
        {
            Relieve();
        }
    }

    private void PlayerMove()   //プレイヤーの移動
    {
        moveVec = shotPos - trans.position; //移動方向算出
        moveVec.y = gravity.groundHitFlg && moveVec.y < 0 ? 0 : moveVec.y;

        //ラインエフェクト
        bulletLineEffect.SetActive(true);
        bulletLineEffect.transform.position = bulletLinePos.transform.position;
        bulletLineEffect.transform.LookAt(shotPos);

        //速さ調整
        nowMoveSpeed += (nowMoveSpeed + moveSpeed) * (nowMoveSpeed + moveSpeed) * Time.deltaTime;
        nowMoveSpeed = nowMoveSpeed > moveSpeedMax ? moveSpeedMax : nowMoveSpeed;   //最大値より大きかったら、最大値にする

        if (Mathf.Abs(moveVec.magnitude) > nowMoveSpeed * Time.deltaTime)  //移動量が大きすぎたら、一定にする
        {
            moveVec = moveVec.normalized * nowMoveSpeed * Time.deltaTime;
        }
        else
        {
            moveFinishFlg = true;
        }

        CC.Move(moveVec);
    }

    private void PlayerHitJudge()   //プレイヤーがオブジェクトに当たっているか
    {
        Ray ray = new Ray(playerTrans.position, moveVec);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, hitRange))
        {
            hitFlg = true;
        }
    }

    private void Relieve()   //解除処理
    {
        nowMoveSpeed = 0;
        metalFlg = false;
        moveFinishFlg = false;
        hitFlg = false;
        return;
    }
}
