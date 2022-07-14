using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private PlayerStartDown playerStartDown;
    private CharacterController CC;
    private MoveWindGun moveWindGun;
    private playerHundLadder ladder;
    private magnet magnet;
    private magnetChain magnetChain;
    private playerDied died;

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

    bool isLanding;     //着地判定
    bool isSoundOn;     //着地サウンド発生
    [SerializeField] private float soundOnHeight;
    [SerializeField] public LayerMask layer;

    void Start()
    {
        //変数を初期化
        GunUseInfo _info = transform.GetComponent<GunUseInfo>();

        CC = transform.GetComponent<CharacterController>();
        moveWindGun = transform.GetComponent<MoveWindGun>();
        ladder = transform.GetComponent<playerHundLadder>();
        magnet = _info.muzzlePos.GetComponent<magnet>();
        magnetChain = _info.muzzlePos.GetComponent<magnetChain>();
        died = transform.GetComponent<playerDied>();
        playerStartDown = transform.GetComponent<PlayerStartDown>();

        trans = transform;
        firstGroundHitFlg = false;
        groundHitFlg = false;
        isLanding = true;
        isSoundOn = false;
    }

    void Update()
    {
        if (ladder.touchLadderFlg || died.diedFlg) return;   //プレイヤーの移動無効化
        if (playerStartDown != null && playerStartDown.isAuto == true) return;

        GravityProcess();

        CC.Move(moveVec * Time.deltaTime);

        //着地状態かチェック
        Vector3 _pos = trans.position + new Vector3(0, -playerHeight, 0);
        Ray ray = new Ray(trans.position, Vector3.down);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, layer);

        if (groundHitFlg && isSoundOn)
        {
            AudioManager.Instance.PlaySE("Landing", isLoop: false);
            isLanding = true;
            isSoundOn = false;
        }

        if(nowGravity <= soundOnHeight)
        {
            isSoundOn = true;
            isLanding = false;
        }
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
        groundHitFlg = false;

        Vector3[] edgeTrans = new Vector3[5];
        for (int i = 0; i < edgeTrans.Length; i++)
        {
            edgeTrans[i] = trans.position;
            edgeTrans[i] += new Vector3(0, -playerHeight, 0);
        }
        edgeTrans[1] += new Vector3(playerRadius, playerHeightGap, 0);
        edgeTrans[2] += new Vector3(-playerRadius, playerHeightGap, 0);
        edgeTrans[3] += new Vector3(0, playerHeightGap, playerRadius);
        edgeTrans[4] += new Vector3(0, playerHeightGap, -playerRadius);

        foreach (Vector3 i in edgeTrans)
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

                if (hit.distance < distanceMin)
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
        nowGravity += gravity * Time.deltaTime;
        moveVec.y += nowGravity;
    }

    public float GetGravity //重力のゲッター
    {
        get { return gravity; }
    }
}
