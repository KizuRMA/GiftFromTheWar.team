using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windHit : MonoBehaviour
{
    private Transform trans;
    [SerializeField] private float movePowerBase;
    [SerializeField] private float movePowerMax;
    [SerializeField] private float movePowerMin;
    private float movePower;

    void Start()
    {
        trans = transform;
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody RD = other.attachedRigidbody;

        if (RD == null || RD.isKinematic) return;   //�����Ȃ��I�u�W�F�N�g�������珈�����Ȃ�

        Vector3 moveVec = RD.gameObject.transform.position - trans.position;    //�ړ��������Z�o

        //�ړ�����͂̌v�Z
        movePower = movePowerBase - moveVec.magnitude;
        movePower = movePower > movePowerMax ? movePowerMax : movePower;
        movePower = movePower < movePowerMin ? movePowerMin : movePower;

        moveVec = moveVec.normalized * movePower;   //�x�N�g������

        RD.AddForce(moveVec, ForceMode.Impulse);
    }
}
