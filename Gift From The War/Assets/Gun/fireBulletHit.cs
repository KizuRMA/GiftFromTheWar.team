using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBulletHit : MonoBehaviour
{
    private Transform trans;
    public GameObject targetImageObj = null;

    [SerializeField] private float scaleSpeed;
    [SerializeField] private float scaleFirst;
    [SerializeField] private float scaleMax;
    private float nowScale;

    //�I�u�W�F�N�g�̈ړ�
    [SerializeField] private float movePowerBase;
    [SerializeField] private float movePowerMax;
    [SerializeField] private float movePowerMin;
    private float movePower;

    void Start()
    {
        trans = transform;
        nowScale = scaleFirst;
    }

    void Update()
    {
        nowScale += scaleSpeed * Time.deltaTime;

        nowScale = nowScale > scaleMax ? scaleMax : nowScale;

        trans.localScale = new Vector3(nowScale, nowScale, nowScale);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bat" || other.gameObject.tag == "Dog1" || other.gameObject.tag == "Boss")
        {
            var enemyInter = other.GetComponent<EnemyInterface>();

            if (enemyInter != null)
            {
                enemyInter.ExpDamage(1,transform.position);

                if (targetImageObj != null)
                {
                    targetImageObj.GetComponent<TargetSetting>().HitAnime();
                }
            }
        }
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
