using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] private CharacterController CC;
    [SerializeField] private MoveWindGun moveWindGun;
    [SerializeField] private playerHundLadder ladder;
    [SerializeField] private magnet magnet;
    [SerializeField] private magnetChain magnetChain;
    [SerializeField] private playerDied died;

    public bool firstGroundHitFlg { get; set; }

    public bool groundHitFlg { get; set; }

    //重力
    private Transform trans;
    private Vector3 moveVec; // 合成用
    [SerializeField] private float gravity;
    private float nowGravity;
    [SerializeField] private float slipAng; //滑る斜面の基準
    private float distanceMin;  //最小の距離を保管
    private Vector3 rayNormal;  //例が当たったところの法線
    [SerializeField] private float groundDis;       //地面との距離
    [SerializeField] private float playerRadius;    //プレイヤーの半径
    [SerializeField] private float playerHeight;    //プレイヤーの高さの補正
    [SerializeField] private float playerHeightGap; //プレイヤーの高さの差
    [SerializeField] private float slipY;           //スリップの高さ
    [SerializeField] private float slipPower;       //スリップするときの移動量

    void Start()
    {
        trans = transform;
        firstGroundHitFlg = false;
        groundHitFlg = false;
    }

    void Update()
    {
        if (ladder.touchLadderFlg || died.diedFlg) return;   //プレイヤーの移動無効化

        GravityProcess();

        CC.Move(moveVec * Time.deltaTime);
    }

    private void GravityProcess()
    {
        moveVec = Vector3.zero;
        distanceMin = float.MaxValue;
        rayNormal = Vector3.zero;

        if (moveWindGun.upWindFlg)  //風の力を使っていたら、重力をリセット
        {
            nowGravity = 0;
            return;
        }

        RayJudge();

    }

    private void RayJudge() //地面のレイ判定
    {
        Vector3[] edgeTrans = new Vector3[5];
        for(int i = 0;i < edgeTrans.Length;i++)
        {
            edgeTrans[i] = trans.position;
            edgeTrans[i] += new Vector3(0, -playerHeight, 0);
        }
        edgeTrans[1] += new Vector3(playerRadius, playerHeightGap, 0);
        edgeTrans[2] += new Vector3(-playerRadius, playerHeightGap, 0);
        edgeTrans[3] += new Vector3(0, playerHeightGap, playerRadius);
        edgeTrans[4] += new Vector3(0, playerHeightGap, -playerRadius);

        foreach(Vector3 i in edgeTrans)
        {
            Ray ray = new Ray(i, -trans.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, groundDis))
            {
                nowGravity = 0;

                groundHitFlg = true;

                if (!firstGroundHitFlg)
                {
                    firstGroundHitFlg = true;
                }

                if (i.x == trans.position.x && i.z == trans.position.z) continue;

                if(hit.distance < distanceMin)
                {
                    rayNormal = hit.normal.normalized;
                }
            }
        }

        if (rayNormal != Vector3.zero)
        {
            if (Mathf.Abs((new Vector3(0, 1, 0) - rayNormal).magnitude) > slipAng)  //斜面の角度が一定以上だったら
            {
                Vector3 slipVec = new Vector3(rayNormal.x, slipY, rayNormal.z);    //ずり落ちる処理
                moveVec += slipVec.normalized * slipPower;
            }
        }

        if (distanceMin != float.MaxValue) return;

        firstGroundHitFlg = false;
        groundHitFlg = false;

        nowGravity += gravity * Time.deltaTime;
        moveVec.y += nowGravity;
    }

    public float GetGravity //重力のゲッター
    {
        get { return gravity; }
    }
}
