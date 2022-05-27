using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWindGun : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    private CharacterController CC;
    private Transform trans;
    [SerializeField] private GameObject cam;
    [SerializeField] private FPSController fpsCon;
    [SerializeField] private playerHundLadder ladder;
    [SerializeField] private remainingAmount energyAmount;
    [SerializeField] private playerDied died;

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

    // Start is called before the first frame update
    void Start()
    {
        CC = this.GetComponent<CharacterController>();
        trans = transform;
        upWindFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (died.diedFlg) return;
        if (ladder.touchLadderFlg) return;

        KnowViewpoint();

        Move();
    }

    private void KnowViewpoint() //どこむいているか
    {
        if (!Input.GetMouseButton(0)) return;  //クリックしていなかったら処理を行わない
        if (energyAmount.GetSetNowAmount <= 0) return;  //エネルギーの残量がなかったら処理を行わない

        viewpoint = Quaternion.Euler(cam.transform.localRotation.eulerAngles.x, trans.localRotation.eulerAngles.y, 0);  //向いている方向計算
    }

    private void Move() //移動の処理
    {
        if (!Input.GetMouseButton(0) || energyAmount.GetSetNowAmount <= 0)   //クリックしていなかったら、またはエネルギー残量がなかったら
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

        if (fpsCon.groundFlg)  //設地判定
        {
            groundFlg = true;
        }

        if (groundFlg)  //空気抵抗の処理
        {
            airResistance -= airResistancePower * Time.deltaTime;
        }

        if (airResistance < 0)   //空気抵抗終了
        {
            airResistance = 0;
            groundFlg = false;
        }

        //空気抵抗の移動量の計算
        power = viewpoint * new Vector3(0, 0, -movePower) * airResistance * Time.deltaTime;
        power.y = 0;
        CC.Move(power);

        energyAmount.GetSetNowAmount = 0;   //エネルギー消費量
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

        energyAmount.GetSetNowAmount = useEnergyAmount; //エネルギー消費
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
}