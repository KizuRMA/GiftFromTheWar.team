using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDied : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    private CharacterController CC;
    private Transform trans;
    [SerializeField] private GameObject rantan;
    [SerializeField] private GameObject gun;
    private Rigidbody rantanRD;
    private Rigidbody gunRD;

    public bool diedFlg { get; set; }

    //�ړ�
    [SerializeField] private float downSpeed;   //������X�s�[�h
    [SerializeField] private float downMax;     //������ő�l
    private float downSum = 0;                  //�_�E���������v�l

    //��]
    [SerializeField] private float rotSpeed;    //��]�X�s�[�h
    [SerializeField] private float rotMax;      //��]�̍ő�l
    private float rotSum = 0;                   //��]�̍��v�l
    [SerializeField] private float gunRotSpeed; //�e�̉�]�X�s�[�h

    void Start()
    {
        CC = this.GetComponent<CharacterController>();
        trans = transform;
        rantanRD = rantan.GetComponent<Rigidbody>();
        gunRD = gun.GetComponent<Rigidbody>();
        diedFlg = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            diedFlg = true;

            CC.enabled = false; //�v���C���[�̓����蔻��폜

            //�e�q�֌W�폜
            rantan.transform.parent = null;
            gun.transform.parent = null;

            //�ړ��p�x�����폜
            rantan.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            gun.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        if (!diedFlg) return;

        EraseInertia();

        DownKnees();

        Fall();
    }

    private void EraseInertia() //����������
    {
        rantanRD.velocity = Vector3.zero;
        rantanRD.angularVelocity = Vector3.zero;
        gunRD.velocity = Vector3.zero;
        gunRD.angularVelocity = Vector3.zero;
    }

    private void DownKnees()    //�G����
    {
        if (downSum >= downMax) return;

        downSum += downSpeed * Time.deltaTime;  //�����鍇�v��ۑ�
        trans.position += new Vector3(0, -downSpeed, 0) * Time.deltaTime;   //�v���C���[���ړ�

        //�I�u�W�F�N�g�����R����
        rantan.transform.position += new Vector3(0, -downSpeed, 0) * Time.deltaTime;
        gun.transform.position += new Vector3(0, -downSpeed, 0) * Time.deltaTime;
    }

    private void Fall()
    {
        if (downSum < downMax) return;

        if (rotSum >= rotMax) return;

        rotSum += rotSpeed * Time.deltaTime;    //��]�̍��v��ۑ�
        trans.rotation *= Quaternion.Euler(rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime);    //�v���C���[����]
        gun.transform.rotation *= Quaternion.Euler(0, 0, gunRotSpeed * Time.deltaTime); //�e����]
    }
}
