using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class magnet : ShootParent
{
    //ゲームオブジェクトやスクリプト
    [SerializeField] private GameObject cameraObj;
    [SerializeField] private GameObject bulletLineEffect;
    private GameObject bulletLinePos;
    private bulletChange bulletChange;
    [SerializeField] private GetItem getItem;
    private magnetChain magnetChain;
    [SerializeField] private Image magnetTarget;
    [SerializeField] private float targetDis;

    //弾の発射
    private bool shotFlg;                       //発射可能
    private bool changeTargetFlg = false;

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

    //壁すり抜け防止
    private List<GameObject> colliders = new List<GameObject>();    //当たり判定のゲームオブジェクト格納
    private List<Vector3> pastPosList = new List<Vector3>();        //当たり判定の座標を保存
    private Vector3 pastPos;                                        //オブジェクトの座標を保存
    private Quaternion pastQua;                                     //オブジェクトの角度を保存

    private void Start()
    {
        //変数初期化
        bulletLinePos = transform.Find("muzzlePosLine").gameObject;

        if (transform.parent != null)
        {
            bulletChange = transform.parent.GetComponent<bulletChange>();
        }

        magnetChain = transform.GetComponent<magnetChain>();

        trans = transform;
        metal = null;
        bulletLineEffect.SetActive(false);
    }

    public void Finish()    //武器を切り替えた時の終了処理
    {
        if (metal != null)
        {
            metal.transform.parent = null;
            AddInertia();
        }
        metal = null;
        bulletLineEffect.SetActive(false);
        nowReturnSpeed = 0;
        shotFlg = false;
        magnetFlg = false;
        cameraOverFlg = false;
    }

    void Update()
    {
        if (!getItem.magnetAmmunitionFlg) return;   //弾を拾ってなかったら処理しない

        if (magnetChain.metalFlg) return;   //すでに別の磁石を打っていたら処理しない

        MoveBullet();

        if (bulletChange.nowBulletType != bulletChange.bulletType.e_magnet || bulletChange.cylinder.isChanging == true) return; //今の弾の種類が対応してなかったら

        //エネルギーが必要量あれば
        shotFlg = energyAmount.GetSetNowAmount > (1.0f - useEnergy);

        //エネルギーを使用しないときは0にする
        if (!Input.GetMouseButtonDown(1) || energyAmount.GetSetNowAmount <= 0 || cameraOverFlg)
        {
            energyAmount.GetSetNowAmount = 0;
        }

        MagnetGuid();

        //発射キーを押したら
        if (Input.GetMouseButtonDown(1))
        {
            if (shotFlg && !magnetFlg)
                Shot();
        }

        //発射した弾が金属に当たってなかったら、処理しない
        if (metal == null)
        {
            bulletLineEffect.SetActive(false);
            return;
        }

        CatchMetal();
    }

    private void MagnetGuid()   //磁石が使えるかどうか
    {
        //プレイヤーの前にレイ判定を飛ばし、オブジェクトとの距離を求める。
        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, targetDis))
        {
            if (hit.transform.gameObject.tag == "metal" || hit.transform.gameObject.tag == "gimmickButton")
            {
                if (!changeTargetFlg)
                {
                    changeTargetFlg = true;
                    magnetTarget.color += new Color(0, 0, 100, 0);
                }
            }
            else
            {
                if (changeTargetFlg)
                {
                    changeTargetFlg = false;
                    magnetTarget.color += new Color(0, 0, -100, 0);
                }
            }
        }
        else
        {
            if (changeTargetFlg)
            {
                changeTargetFlg = false;
                magnetTarget.color += new Color(0, 0, -100, 0);
            }
        }
    }

    private void Shot() //弾を打つ処理
    {
        AudioManager.Instance.PlaySE("磁石発射", false, vol: 0.5f);

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

    private void CatchMetal()   //金属を操る
    {
        if (!magnetFlg) //最初に金属に触れたときだけ行う
        {
            magnetFlg = true;
            cameraOverFlg = false;
            metal.transform.parent = cameraObj.gameObject.transform;
            firstPos = metal.transform.localPosition;
            pastPos = metal.transform.position;
            pastQua = metal.transform.rotation;

            Rigidbody _rd = metal.GetComponent<Rigidbody>();
                
            if (_rd == null) _rd = metal.gameObject.AddComponent<Rigidbody>();

            _rd.useGravity = false;
            metal.gameObject.AddComponent<metalHitJudge>();

            if (metal.GetComponent<Rigidbody>().isKinematic)
            {
                metal.GetComponent<Rigidbody>().isKinematic = false;
            }

            ColliderInit();
        }

        AudioManager.Instance.PlaySE("溶接", vol: 0.7f);

        ReturnMiddle();
        EraseInertia();

        //ラインエフェクト
        bulletLineEffect.SetActive(true);
        bulletLineEffect.transform.position = bulletLinePos.transform.position;
        bulletLineEffect.transform.LookAt(metal.transform.position);

        //エネルギー消費
        energyAmount.GetSetNowAmount = useEnergyMag;
        energyAmount.useDeltaTime = true;

        CameraOver();

        ThroughWall();

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

    private void ColliderInit() //当たり判定用のゲームオブジェクトの初期化
    {
        GameObject colliderChild = metal.transform.Find("collider").gameObject;
        for (int i = 0; i < colliderChild.transform.childCount; i++)
        {
            colliders.Add(colliderChild.transform.GetChild(i).gameObject);
            pastPosList.Add(colliders[i].transform.position);
        }
    }

    private void ThroughWall()  //壁を通り抜ける
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            Vector3 vec = colliders[i].transform.position - pastPosList[i];

            Ray ray = new Ray(pastPosList[i], vec);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, vec.magnitude))
            {
                if (hit.collider.gameObject.tag == "cave")
                {
                    metal.transform.position = pastPos;
                    metal.transform.rotation = pastQua;
                }
            }

            pastPosList[i] = colliders[i].transform.position;
        }

        pastPos = metal.transform.position;
        pastQua = metal.transform.rotation;
    }

    private void Relieve()   //解除処理
    {
        //解除する処理
        if (Input.GetMouseButtonDown(1) || energyAmount.GetSetNowAmount <= 0 || cameraOverFlg)
        {
            AudioManager.Instance.StopSE("溶接");

            magnetFlg = false;
            metal.transform.parent = null;
            AddInertia();
            metal = null;
            colliders.Clear();
            pastPosList.Clear();
        }
    }
}
