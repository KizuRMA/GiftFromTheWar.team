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

        if (RD == null || RD.isKinematic) return;   //動かないオブジェクトだったら処理しない

        Vector3 moveVec = RD.gameObject.transform.position - trans.position;    //移動方向を算出

        //移動する力の計算
        movePower = movePowerBase - moveVec.magnitude;
        movePower = movePower > movePowerMax ? movePowerMax : movePower;
        movePower = movePower < movePowerMin ? movePowerMin : movePower;

        moveVec = moveVec.normalized * movePower;   //ベクトル生成

        RD.AddForce(moveVec, ForceMode.Impulse);
    }
}
