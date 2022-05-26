using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rantanWallTouch : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    private Transform trans;
    private Rigidbody rd;

    //�t���O
    private bool returnFlg = true;              //�߂邩
    public bool returnFinishFlg { get; set; }   //�߂�I�������

    //�ړ�
    private Vector3 firstPos;                   //��̈ʒu
    [SerializeField] private float firstSpeed;  //�ŏ��̑���
    [SerializeField] private float speedRaito;  //�����̔{��
    private float nowSpeed;                     //���̑���
    private Vector3 forceVec;                   //�ړ��̌���

    void Start()
    {
        trans = transform;
        rd = this.GetComponent<Rigidbody>();
        firstPos = transform.localPosition;
        returnFinishFlg = true;
    }

    void Update()
    {
        EraseInertia();

        //�����߂�����
        if (returnFlg)
        {
            Return();
        }
    }

    private void EraseInertia() //����������
    {
        rd.velocity = Vector3.zero;
        rd.angularVelocity = Vector3.zero;
    }

    private void Return() //�߂�����
    {
        ReturnSpeedUp();

        CalculateForceVec();

        ForceVecCorrection();

        trans.localPosition += forceVec;
    }

    private void ReturnSpeedUp() //�߂������𑬂�����
    {
        nowSpeed *= speedRaito;
    }

    private void CalculateForceVec() //forceVec�̌v�Z  
    {
        forceVec = firstPos - trans.localPosition;
    }

    private void ForceVecCorrection() //�Ԃ�Ԃ邵�Ȃ����߂̕␳
    {
        bool froceVecSmallFlg = forceVec.magnitude > nowSpeed;  //forceVec�����������Ȃ�������
        if (froceVecSmallFlg)
        {
            forceVec = forceVec.normalized * nowSpeed;
        }
        else
        {
            nowSpeed = firstSpeed;
            returnFlg = false;
            returnFinishFlg = true;
        }
    }

    private void OnCollisionEnter(Collision collison)   //�Փ�
    {
        returnFlg = false;
        returnFinishFlg = false;
        nowSpeed = firstSpeed;
    }

    private void OnCollisionExit(Collision collison)    //�ՓˏI��
    {
        returnFlg = true;
    }
}
