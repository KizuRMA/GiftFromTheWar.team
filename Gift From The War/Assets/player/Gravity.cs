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
    [SerializeField] private float groundDis;   //地面との距離

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

        if (moveWindGun.upWindFlg)
        {
            nowGravity = gravity * Time.deltaTime;
            return;
        }

        Ray ray = new Ray(trans.position, -trans.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, groundDis) || CC.isGrounded)   //地面についている場合
        {
            groundHitFlg = true;

            Debug.Log(hit.distance);

            nowGravity = gravity * Time.deltaTime;

            if (firstGroundHitFlg) return;
            moveVec.y += 1.0f;  //完全に地面につけるための処理
            firstGroundHitFlg = true;
            return;
        }
        else
        {
            firstGroundHitFlg = false;
            groundHitFlg = false;
        }

        nowGravity += gravity * Time.deltaTime;
        moveVec.y += nowGravity;
    }

    public float GetGravity //重力のゲッター
    {
        get { return gravity; }
    }
}
