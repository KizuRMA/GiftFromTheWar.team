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
    private List<GameObject> bullet = new List<GameObject>();   //弾の配列
    [SerializeField] private float shotSpeed;   //発射スピード
    [SerializeField] private float range;       //弾の消えるまでの時間
    [SerializeField] private float useEnergy;   //消費エネルギー
    private bool shotFlg;                       //発射可能
    private Vector3 shotPos;                    //着弾点

    //移動処理
    public bool metalFlg { get; set; }              //金属にくっついたフラグ
    [SerializeField] private float moveSpeed;       //移動する速さ
    private Vector3 moveVec;                        //移動方向
    private bool moveFinishFlg = false;             //移動が終わったフラグ
    private bool hitFlg = false;                    //オブジェクトにあたったか
    private Vector3 prePos;                         //前フレームの位置を記憶しておく
    private bool useEnergy0;                        //エネルギー消費量を0にするフラグ
    [SerializeField] private float hitRange;        //当たり判定の範囲

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

        MoveBullet();

        //発射キーを押したら
        if (Input.GetMouseButtonDown(0))
        {
            if (!shotFlg) return;
            if (metalFlg) return;
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
        //リストに弾を追加
        bullet.Add((GameObject)Instantiate(bulletPrefab, trans.position, Quaternion.identity));
        Rigidbody bulletRb = bullet[bullet.Count - 1].GetComponent<Rigidbody>();    //リジッドボディ追加

        //目的地に球を方向転換
        bullet[bullet.Count - 1].transform.LookAt(shotPos);       

        //射撃されてから指定秒後に銃弾のオブジェクトを破壊する
        Destroy(bullet[bullet.Count - 1], range);
    }

    private void MoveBullet()   //弾の移動
    {
        if (bullet == null) return; //弾がなければ処理しない

        for(int i = 0;i < bullet.Count;i++) //弾の数だけ繰り返す
        {
            if(bullet[i] == null)   //弾が破壊されていたら、リストから削除
            {
                bullet.RemoveAt(i);
                continue;
            }
            bullet[i].transform.transform.position += bullet[i].transform.forward * shotSpeed * Time.deltaTime; //移動処理
        }
    }

    private void MagnetChain()   //金属の方に飛ぶ
    {
        prePos = playerTrans.position;
        PlayerMove();
        PlayerHitJudge();        

        //解除する処理
        if (moveFinishFlg || hitFlg)
        {
            Relieve();
        }
    }

    private void PlayerMove()   //プレイヤーの移動
    {
        moveVec = shotPos - trans.position; //移動方向算出

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
        metalFlg = false;
        moveFinishFlg = false;
        hitFlg = false;
        return;
    }
}
