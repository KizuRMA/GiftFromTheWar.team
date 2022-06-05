using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDied : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    private CharacterController CC;
    private Transform trans;
    [SerializeField] private Gravity gravity;
    [SerializeField] private GameObject rantan;
    [SerializeField] private GameObject gun;
    private Rigidbody rantanRD;
    private Rigidbody gunRD;

    public bool diedFlg { get; set; }

    //�ړ�
    [SerializeField] private float height;      //����
    [SerializeField] private float downSpeed;   //������X�s�[�h
    [SerializeField] private float downMax;     //������ő�l
    private float nowGravity;                   //���̏d�͉����x

    //��]
    [SerializeField] private float rotSpeed;    //��]�X�s�[�h
    [SerializeField] private float rotMax;      //��]�̍ő�l
    private float rotSum = 0;                   //��]�̍��v�l
    [SerializeField] private float gunRotSpeed; //�e�̉�]�X�s�[�h

    private bool groundFlg = false;    //��x�ł��n�ʂɂ������ǂ���

    void Start()
    {
        CC = this.GetComponent<CharacterController>();
        trans = transform;
        rantanRD = rantan.GetComponent<Rigidbody>();
        gunRD = gun.GetComponent<Rigidbody>();
        diedFlg = false;
        nowGravity = gravity.GetGravity * Time.deltaTime;
    }

    void Update()
    {
        if (CC.GetComponent<playerAbnormalcondition>().life <= 0)
        {
            diedFlg = true;

            CC.height = height;

            //�e�q�֌W�폜
            rantan.transform.parent = null;
            gun.transform.parent = null;

            rantanRD.useGravity = true;
            gunRD.useGravity = true;

            //�ړ��p�x�����폜
            rantanRD.constraints = RigidbodyConstraints.None;
            gunRD.constraints = RigidbodyConstraints.None;
        }

        if (!diedFlg) return;

        DownKnees();

        Fall();
    }

    private void DownKnees()    //�G����
    {
        //���C����Œn�ʂɒ��������m�F����
        Ray ray = new Ray(trans.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, downMax))
        {
            groundFlg = true;
        }

        nowGravity += gravity.GetGravity * Time.deltaTime;
        CC.Move(new Vector3(0, nowGravity, 0) * Time.deltaTime);   //�v���C���[���ړ�

    }

    private void Fall()
    {
        if (!groundFlg) return;
        if (rotSum > rotMax) return;

        rotSum += rotSpeed * Time.deltaTime;    //��]�̍��v��ۑ�
        trans.rotation *= Quaternion.Euler(rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime);    //�v���C���[����]
        gun.transform.rotation *= Quaternion.Euler(0, 0, gunRotSpeed * Time.deltaTime); //�e����]
    }
}
