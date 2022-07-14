using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWindGun : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    private CharacterController CC;
    private Transform trans;
    [SerializeField] private GameObject cam;
    private Gravity gravity;
    private playerHundLadder ladder;
    private remainingAmount energyAmount;
    private playerDied died;
    private bulletChange bulletChange;
    private GetItem getItem;
    private shooting shoot;
    private PlayerStartDown playerStartDown;

    //移動
    [SerializeField] private float movePower;
    [SerializeField] private float movePowerMin;    //パワーの最低値
    [SerializeField] private float range;           //空気が届く射程
    private float disRaitoPower;                    //距離による補正
    [SerializeField] private float useEnergyAmount; //消費エネルギー量
    private Quaternion viewpoint;                   //向いてる方向
    private Vector3 power;                          //最終的な移動量

    //空気抵抗
    private bool groundFlg = false;                     //地面についているか
    private float airResistance;                        //現在の空気抵抗
    [SerializeField] private float airResistancePower;  //空気抵抗量
    [SerializeField] private float airResistanceMax;    //空気抵抗の最大値
    [SerializeField] private float airResistanceMin;    //空気抵抗の最小値

    public bool upWindFlg { get; set; }
    public bool effectFlg { get; set; }

    void Start()
    {
        //変数を初期化
        GunUseInfo _info = transform.GetComponent<GunUseInfo>();

        gravity = transform.GetComponent<Gravity>();
        ladder = transform.GetComponent<playerHundLadder>();
        energyAmount = _info.cube.GetComponent<remainingAmount>();
        died = transform.GetComponent<playerDied>();
        bulletChange = _info.gunModel.GetComponent<bulletChange>();
        getItem = transform.GetComponent<GetItem>();
        shoot = _info.muzzlePos.GetComponent<shooting>();
        playerStartDown = transform.GetComponent<PlayerStartDown>();

        CC = this.GetComponent<CharacterController>();
        trans = transform;
        upWindFlg = false;
        effectFlg = false;
    }

    public void Finish()    //武器を切り替えた時の終了処理
    {
        upWindFlg = false;
        effectFlg = false;
    }

    void Update()
    {
        if (!getItem.windAmmunitionFlg) return; //弾を拾ってなかったら処理しない
        if (playerStartDown != null && playerStartDown.isAuto == true) return;

        if (bulletChange.nowBulletType != bulletChange.bulletType.e_wind || bulletChange.cylinder.isChanging == true) return;   //今の弾の種類が対応してなかったら

        if (died.diedFlg) return;
        if (ladder.touchLadderFlg) return;

        KnowViewpoint();

        Move();
    }

    private void KnowViewpoint() //どこむいているか
    {
        effectFlg = false;
        if (!Input.GetMouseButton(0)) return;  //クリックしていなかったら処理を行わない
        if (energyAmount.GetSetNowAmount <= 0) return;  //エネルギーの残量がなかったら処理を行わない

        viewpoint = Quaternion.Euler(cam.transform.localRotation.eulerAngles.x, trans.localRotation.eulerAngles.y, 0);  //向いている方向計算
        effectFlg = true;
        AudioManager.Instance.PlaySE("Wind", isLoop: false);

    }

    private void Move() //移動の処理
    {
        if ((!Input.GetMouseButton(0)) || energyAmount.GetSetNowAmount <= 0)   //クリックしていなかったら、またはエネルギー残量がなかったら
        {
            AirResistance();
            return;
        }

        if (energyAmount.GetSetNowAmount <= 0) return;

        WindMove();
    }

    private void AirResistance()    //空気抵抗処理
    {
        upWindFlg = false;

        if (gravity.firstGroundHitFlg)  //設地判定
        {
            groundFlg = true;
        }

        if (groundFlg)  //空気抵抗の処理
        {
            airResistance -= airResistancePower * Time.deltaTime;
        }

        airResistance -= airResistancePower * Time.deltaTime;

        if (airResistance < 0)   //空気抵抗終了
        {
            airResistance = 0;
            groundFlg = false;
        }

        //空気抵抗の移動量の計算
        power = viewpoint * new Vector3(0, 0, -movePower) * airResistance * Time.deltaTime;
        power.y = 0;
        CC.Move(power);

        if (shoot.shotFlg) return;  //弾を発射していたら、エネルギー消費量を0にしない
        energyAmount.GetSetNowAmount = 0;   //エネルギー消費量を0にする
    }

    private void WindMove() //風の移動の処理
    {
        upWindFlg = true;

        CorrectionDis();

        //風の移動量の計算
        power = viewpoint * new Vector3(0, 0, -movePower) * disRaitoPower * Time.deltaTime;
        CC.Move(power);

        //銃の向きが下を向いていなかったら、上に浮く力をオフする
        if (cam.transform.localRotation.eulerAngles.x < 40)
        {
            upWindFlg = false;
        }

        //エネルギー消費
        energyAmount.GetSetNowAmount = useEnergyAmount;
        energyAmount.useDeltaTime = true;
    }

    private void CorrectionDis()    //地面からどれだけ離れているか
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);   //プレイヤーの下にレイ判定を飛ばす
        RaycastHit hit;
        int layerMask = 1 << 9;
        if (Physics.Raycast(ray, out hit, range, layerMask))    //地面から一定距離内にいたら
        {
            disRaitoPower = 1.0f - hit.distance / range + movePowerMin; //離れている距離に応じて、風の強さが変わる

            //空気抵抗の計算のときのために、値を保持する
            airResistance = disRaitoPower;
            if (airResistance < airResistanceMin)
            {
                airResistance = airResistanceMin;
            }

            if(airResistance > airResistanceMax)
            {
                airResistance = airResistanceMax;
            }
        }
        else
        {
            disRaitoPower = 0;
        }
    }

    private void ObjectsAffect()    //風によるオブジェクトへの影響
    {
        //GameObject stage = GameObject.FindGameObjectWithTag("stage");
        //if (stage == null) return;

        //List<GameObject> gameList = new List<GameObject>();

        //GameObject game;
        //game = stage.transform.Find("dynamicObj").gameObject;

        ////プレイヤーと近いステージオブジェクトをリストに保管
        //for (int i  = 0; i < game.transform.childCount; i++)
        //{
        //    for (int j = 0; j < game.transform.GetChild(i).childCount; j++)
        //    {
        //        Transform trans = game.transform.GetChild(i).GetChild(j).transform;
        //        float dis = Vector3.Distance(trans.position,CC.transform.position);

        //        if (dis < range)
        //        {
        //            gameList.Add(game.transform.GetChild(i).GetChild(j).gameObject);
        //        }
        //    }
        //}

        //if (gameList.Count == 0) return;

        //for (int i = 0; i < gameList.Count; i++)
        //{
        //    //ターゲットへのベクトル
        //    //Vector3 _targetVec = gameList[i].transform.position - CC.transform.position;
        //    //float dis

        //}

    }
}