using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnet : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    private Transform trans;
    [SerializeField] private Transform camTrans;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private remainingAmount energyAmount;
    [SerializeField] private GameObject cameraObj;

    //弾の発射
    [SerializeField] private float shotSpeed;   //発射スピード
    [SerializeField] private float range;       //弾の消えるまでの時間
    [SerializeField] private float useEnergy;   //消費エネルギー
    private bool shotFlg;                       //発射可能
    private Vector3 shotPos;                    //着弾点

    //磁石の処理
    public GameObject metal { get; set; }           //くっついた金属
    private Vector3 firstPos;                       //金属の最初のZ座標
    private float nowReturnSpeed;                   //今の戻る速さ
    [SerializeField] private float returnSpeed;     //戻る速さ
    [SerializeField] private float returnSpeedMin;  //戻る速さの最小値
    [SerializeField] private float returnSpeedMax;  //戻る速さの最小値
    [SerializeField] private float useEnergyMag;    //くっついているときに消費するエネルギー
    private bool magnetFlg = false;                 //磁石が触れた瞬間のフラグ
    private bool cameraOverFlg = false;             //金属がカメラ外にでた時
    [SerializeField] private float cameraOverMax;   //カメラの外の上限
    public float sensityvity;                       //カメラの感度

    private void Start()
    {
        trans = transform;
        metal = null;
    }

    void Update()
    {
        //エネルギーが必要量あれば
        shotFlg = energyAmount.GetSetNowAmount > (1.0f - useEnergy);

        //エネルギーを使用しないときは0にする
        if (!Input.GetMouseButtonDown(1) || energyAmount.GetSetNowAmount <= 0 || cameraOverFlg)
        {
            energyAmount.GetSetNowAmount = 0;
        }

        //発射キーを押したら
        if (Input.GetMouseButtonDown(1))
        {
            if(shotFlg && !magnetFlg)
            Shot();
        }

        //発射した弾が金属に当たってなかったら、処理しない
        if (metal == null) return;

        CatchMetal();
    }

    private void Shot() //弾を打つ処理
    {
        energyAmount.GetSetNowAmount = useEnergy;
        energyAmount.useDeltaTime = false;

        BulletVecter();

        CreateBullet();
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
        bulletRb.AddForce(trans.forward * shotSpeed * Time.deltaTime);

        //射撃されてから指定秒後に銃弾のオブジェクトを破壊する
        Destroy(bullet, range);
    }

    private void CatchMetal()   //金属を操る
    {
        if (!magnetFlg) //最初に金属に触れたときだけ行う
        {
            magnetFlg = true;
            cameraOverFlg = false;
            metal.transform.parent = cameraObj.gameObject.transform;
            firstPos = metal.transform.localPosition;
            metal.GetComponent<Rigidbody>().useGravity = false;
            metal.gameObject.AddComponent<metalHitJudge>();
        }

        ReturnMiddle();
        EraseInertia();

        //エネルギー消費
        energyAmount.GetSetNowAmount = useEnergyMag;
        energyAmount.useDeltaTime = true;

        CameraOver();

        Relieve();
    }

    private void EraseInertia() //慣性を消す
    {
        metal.GetComponent<Rigidbody>().velocity = Vector3.zero;
        metal.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void AddInertia() //慣性をつける
    {
        metal.GetComponent<Rigidbody>().useGravity = true;
    }

    private void ReturnMiddle() //真ん中に戻る処理
    {
        Vector3 returnVec = metal.transform.localPosition - firstPos;   //戻る方向を算出

        ReturnSpeed();

        bool returnVecLarge = returnVec.magnitude > nowReturnSpeed * Time.deltaTime;   //戻るベクトルが大きすぎないか
        if (returnVecLarge)
        {
            returnVec = returnVec.normalized * nowReturnSpeed * Time.deltaTime;
        }
        metal.transform.localPosition -= returnVec;
    }

    private void ReturnSpeed()  //戻る速さの算出
    {
        //オブジェクトに当たったかどうか（慣性がかかったかどうかで判断する）
        bool inertiaFlg = metal.GetComponent<metalHitJudge>().hitJudge;
        if (inertiaFlg)
        {
            nowReturnSpeed = returnSpeedMin;
        }
        else
        {
            nowReturnSpeed += returnSpeed * Time.deltaTime;
        }

        //上限補正
        if (nowReturnSpeed > returnSpeedMax)
        {
            nowReturnSpeed = returnSpeedMax;
        }
    }

    private void CameraOver()   //カメラの外に出る処理
    {
        cameraOverFlg = (metal.transform.localPosition - firstPos).magnitude > cameraOverMax;
    }

    private void Relieve()   //解除処理
    {
        //解除する処理
        if (Input.GetMouseButtonDown(1) || energyAmount.GetSetNowAmount <= 0 || cameraOverFlg)
        {
            magnetFlg = false;
            metal.transform.parent = null;
            AddInertia();
            metal = null;
        }
    }
}
