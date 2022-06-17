using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rantanLadder : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    [SerializeField] private CharacterController playerCC;
    [SerializeField] private playerHundLadder playerHund;
    [SerializeField] private rantanWallTouch rantanWallTouch;
    [SerializeField] private gunWallTouch gunWallTouch;
    private Transform trans;

    //��q�����
    private Vector3 firstPos;                   //��̈ʒu
    [SerializeField] private float upDownSpeed; //�����^���̈ړ��X�s�[�h
    [SerializeField] private float maxPosY;     //�����^���̍ő�ړ��ʒu

    private bool limitFlg = false;   //�ő�ړ��ʒu�ɓ��B���邩
    private bool returnLimitFlg = true;   //�ŏ��̈ʒu�ɓ��B���邩

    void Start()
    {
        trans = transform;
        firstPos = trans.localPosition;
    }

    void Update()
    {
        if (playerHund.ClimbLadderFlg())
        {
            TouchLadder();
            return;
        }

        NoTouchLadder();
    }

    private void TouchLadder()
    {
        if (limitFlg) return;  //�ő�ړ��ʒu�ɓ��B���Ă�����

        //�ǂ��肪�I����Ă�����
        if (!rantanWallTouch.returnFinishFlg) return;
        if (!gunWallTouch.returnFinishFlg) return;

        returnLimitFlg = false;

        bool maxPosFlg = trans.localPosition.y < firstPos.y - maxPosY; //�ő�ړ��ʒu�ɓ��B����
        if (maxPosFlg)
        {
            limitFlg = true;
            return;
        }

        trans.localPosition += new Vector3(0, -upDownSpeed, 0) * Time.deltaTime;    //�ړ�����
    }

    private void NoTouchLadder()
    {
        if (returnLimitFlg) return;   //���̈ʒu�ɖ߂��Ă�����

        limitFlg = false;

        bool returnFlg = trans.localPosition.y > firstPos.y;    //���̈ʒu�ɖ߂��Ă�����
        if (returnFlg)
        {
            returnLimitFlg = true;
            return;
        }

        trans.localPosition += new Vector3(0, upDownSpeed, 0) * Time.deltaTime; //�߂�
    }
}
