using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnetChain : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    private Transform trans;
    [SerializeField] private CharacterController CC;    
    [SerializeField] private Transform playerTrans;
    [SerializeField] private Transform camTrans;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private remainingAmount energyAmount;

    //�e�̔���
    [SerializeField] private float shotSpeed;   //���˃X�s�[�h
    [SerializeField] private float useEnergy;   //����G�l���M�[
    private bool shotFlg;                       //���ˉ\
    private Vector3 shotPos;                    //���e�_

    //�ړ�����
    public bool metalFlg { get; set; }              //�����ɂ��������t���O
    public bool magnetFlg { get; set; }               //���Έړ����̃t���O
    [SerializeField] private float moveSpeed;       //�ړ����鑬��
    [SerializeField] private float range;   
    private bool moveFinishFlg = false;             //�ړ����I������t���O
    private bool hitFlg = false;                    //�I�u�W�F�N�g�ɂ���������
    private Vector3 prePos;                         //�O�t���[���̈ʒu���L�����Ă���
    private bool useEnergy0;                        //�G�l���M�[����ʂ�0�ɂ���t���O

    private void Start()
    {
        trans = transform;
        metalFlg = false;
    }

    void Update()
    {
        //�G�l���M�[���K�v�ʂ����
        shotFlg = energyAmount.GetSetNowAmount > (1.0f - useEnergy);

        if(useEnergy0)  //�G�l���M�[����ʂ�0�ɂ���
        {
            useEnergy0 = false;
            energyAmount.GetSetNowAmount = 0;
        }

        //���˃L�[����������
        if (Input.GetMouseButton(0))
        {
            if (!shotFlg) return;
            if (magnetFlg) return;
            Shot();
        }

        //���˂����e�������ɓ������ĂȂ�������A�������Ȃ�
        if (!metalFlg) return;

        MagnetChain();
    }

    private void Shot() //�e��ł���
    {
        energyAmount.GetSetNowAmount = useEnergy;
        energyAmount.useDeltaTime = false;

        BulletVecter();

        CreateBullet();

        useEnergy0 = true;
    }

    private void BulletVecter() //�e�̌��������߂�
    {
        //�v���C���[�̑O�Ƀ��C������΂��A�I�u�W�F�N�g�Ƃ̋��������߂�B
        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            shotPos = hit.point;
        }
    }

    private void CreateBullet() //�v���n�u����e�����
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, trans.position, Quaternion.identity);
        trans.LookAt(shotPos);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(trans.forward * shotSpeed);

        //�ˌ�����Ă���3�b��ɏe�e�̃I�u�W�F�N�g��j�󂷂�.
        Destroy(bullet, 3.0f);
    }

    private void MagnetChain()   //�����̕��ɔ��
    {
        RangeCheck();

        if (!magnetFlg) return;

        prePos = playerTrans.position;
        PlayerMove();
        PlayerHitJudge();        

        //�������鏈��
        if (moveFinishFlg || hitFlg || !magnetFlg)
        {
            Relieve();
        }
    }

    private void RangeCheck()   //�˒������ǂ�������
    {
        if (magnetFlg) return;

        bool rangeJudge = Mathf.Abs((trans.position - shotPos).magnitude) < range;
        if (rangeJudge)
        {
            magnetFlg = true;
        }
        else
        {
            Relieve();
        }
    }

    private void PlayerMove()   //�v���C���[�̈ړ�
    {
        Vector3 moveVec = shotPos - trans.position; //�ړ������Z�o

        if (Mathf.Abs(moveVec.magnitude) > moveSpeed * Time.deltaTime)  //�ړ��ʂ��傫��������A���ɂ���
        {
            moveVec = moveVec.normalized * moveSpeed * Time.deltaTime;
        }
        else
        {
            moveFinishFlg = true;
        }

        CC.Move(moveVec);
    }

    private void PlayerHitJudge()
    {
        float moveAmount = Mathf.Abs(playerTrans.position.magnitude) - Mathf.Abs(prePos.magnitude); //�O�̃t���[������̈ړ���

        if(Mathf.Abs(moveAmount) <= 0.0001)   //�ړ��ʂ��A������������A�����������Ă�Ƃ݂Ȃ�
        {
            hitFlg = true;     
        }
    }

    private void Relieve()   //��������
    {
        magnetFlg = false;
        metalFlg = false;
        moveFinishFlg = false;
        hitFlg = false;
        return;
    }
}
