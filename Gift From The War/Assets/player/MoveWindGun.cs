using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWindGun : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    private CharacterController CC;
    private Transform trans;
    [SerializeField] private GameObject cam;
    [SerializeField] private playerHundLadder ladder;
    [SerializeField] private remainingAmount energyAmount;

    //移動
    [SerializeField] private float movePower;
    [SerializeField] private float movePowerMin;    //パワーの最低値
    [SerializeField] private float range;           //空気が届く射程
    private float disRaitoPower;                    //距離による補正
    [SerializeField] private float useEnergyAmount; //消費エネルギー量
    private Quaternion viewpoint;                   //向いてる方向
    private Vector3 power;                          //最終的な移動量
    private float gravity;                          //重力の力

    //空気抵抗
    private bool groundFlg = false;                     //地面についているか
    private float airResistance;                        //現在の空気抵抗
    [SerializeField] private float airResistancePower;  //空気抵抗量
    [SerializeField] private float airResistanceMax;    //空気抵抗の最大値
    [SerializeField] private float airResistanceMin;    //空気抵抗の最小値

    private bool upWindFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        CC = this.GetComponent<CharacterController>();
        trans = transform;
        gravity = this.GetComponent<FPSController>().GetGravity;
    }

    // Update is called once per frame
    void Update()
    {
        if (ladder.GetTouchLadderFlg()) return;

        KnowViewpoint();

        Move();
    }

    private void KnowViewpoint() //どこむいているか
    {
        if (!Input.GetMouseButton(0)) return;  //クリックしていなかったら処理を行わない
        if (energyAmount.GetSetNowAmount <= 0) return;  //エネルギーの残量がなかったら処理を行わない

        viewpoint = Quaternion.Euler(cam.transform.localRotation.eulerAngles.x, trans.localRotation.eulerAngles.y, 0);  //向いている方向計算

        energyAmount.GetSetNowAmount = useEnergyAmount; //エネルギー消費
    }

    private void Move() //移動の処理
    {
        if (!Input.GetMouseButton(0))
        {
            upWindFlg = false;

            if (CC.isGrounded)
            {
                groundFlg = true;
            }

            if (groundFlg)
            {
                airResistance -= airResistancePower * Time.deltaTime;
            }

            if(airResistance < 0)
            {
                airResistance = 0;
                groundFlg = false;
            }

            power = viewpoint * new Vector3(0, 0, -movePower) * airResistance * Time.deltaTime;
            power.y = 0;
            CC.Move(power);

            energyAmount.GetSetNowAmount = 0;

            return;
        }

        if (energyAmount.GetSetNowAmount <= 0) return;

        upWindFlg = true;
        CorrectionDis();

        power = viewpoint * new Vector3(0, 0, -movePower) * disRaitoPower * Time.deltaTime;

        if (cam.transform.localRotation.eulerAngles.x < 40)
        {
            upWindFlg = false;
        }
        CC.Move(power);
    }

    private void CorrectionDis()    //地面からどれだけ離れているか
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        int layerMask = 1 << 9;
        if (Physics.Raycast(ray, out hit, range, layerMask))
        {
            disRaitoPower = 1.0f - hit.distance / range + movePowerMin;

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

    public bool GetUpWindGunFlg()
    {
        return upWindFlg;
    }

    public Vector3 GetPower()
    {
        return power;
    }
}