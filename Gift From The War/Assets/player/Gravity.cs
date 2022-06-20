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

    //�d��
    private Transform trans;
    private Vector3 moveVec; // �����p
    [SerializeField] private float gravity;
    private float nowGravity;
    [SerializeField] private float slipAng; //����Ζʂ̊
    private float distanceMin;  //�ŏ��̋�����ۊ�
    private Vector3 rayNormal;  //�Ⴊ���������Ƃ���̖@��
    [SerializeField] private float groundDis;       //�n�ʂƂ̋���
    [SerializeField] private float playerRadius;    //�v���C���[�̔��a
    [SerializeField] private float playerHeight;    //�v���C���[�̍����̕␳
    [SerializeField] private float playerHeightGap; //�v���C���[�̍����̍�
    [SerializeField] private float slipY;           //�X���b�v�̍���
    [SerializeField] private float slipPower;       //�X���b�v����Ƃ��̈ړ���

    void Start()
    {
        trans = transform;
        firstGroundHitFlg = false;
        groundHitFlg = false;
    }

    void Update()
    {
        if (ladder.touchLadderFlg || died.diedFlg) return;   //�v���C���[�̈ړ�������

        GravityProcess();

        CC.Move(moveVec * Time.deltaTime);
    }

    private void GravityProcess()
    {
        moveVec = Vector3.zero;
        distanceMin = float.MaxValue;
        rayNormal = Vector3.zero;

        if (moveWindGun.upWindFlg)  //���̗͂��g���Ă�����A�d�͂����Z�b�g
        {
            nowGravity = 0;
            return;
        }

        RayJudge();

    }

    private void RayJudge() //�n�ʂ̃��C����
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
            if (Mathf.Abs((new Vector3(0, 1, 0) - rayNormal).magnitude) > slipAng)  //�Ζʂ̊p�x�����ȏゾ������
            {
                Vector3 slipVec = new Vector3(rayNormal.x, slipY, rayNormal.z);    //���藎���鏈��
                moveVec += slipVec.normalized * slipPower;
            }
        }

        if (distanceMin != float.MaxValue) return;

        firstGroundHitFlg = false;
        groundHitFlg = false;

        nowGravity += gravity * Time.deltaTime;
        moveVec.y += nowGravity;
    }

    public float GetGravity //�d�͂̃Q�b�^�[
    {
        get { return gravity; }
    }
}
