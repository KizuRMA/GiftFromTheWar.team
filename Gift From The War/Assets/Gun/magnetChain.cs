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
    [SerializeField] private float range;       //�e�̏�����܂ł̎���
    [SerializeField] private float useEnergy;   //����G�l���M�[
    private bool shotFlg;                       //���ˉ\
    private Vector3 shotPos;                    //���e�_

    //�ړ�����
    public bool metalFlg { get; set; }              //�����ɂ��������t���O
    [SerializeField] private float moveSpeed;       //�ړ����鑬��
    private Vector3 moveVec;                        //�ړ�����
    private bool moveFinishFlg = false;             //�ړ����I������t���O
    private bool hitFlg = false;                    //�I�u�W�F�N�g�ɂ���������
    private Vector3 prePos;                         //�O�t���[���̈ʒu���L�����Ă���
    private bool useEnergy0;                        //�G�l���M�[����ʂ�0�ɂ���t���O
    [SerializeField] private float hitRange;        //�����蔻��͈̔�

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
        if (Input.GetMouseButtonDown(0))
        {
            if (!shotFlg) return;
            if (metalFlg) return;
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
        bulletRb.AddForce(trans.forward * shotSpeed * Time.deltaTime);

        //�ˌ�����Ă���w��b��ɏe�e�̃I�u�W�F�N�g��j�󂷂�
        Destroy(bullet, range);
    }

    private void MagnetChain()   //�����̕��ɔ��
    {
        prePos = playerTrans.position;
        PlayerMove();
        PlayerHitJudge();        

        //�������鏈��
        if (moveFinishFlg || hitFlg)
        {
            Relieve();
        }
    }

    private void PlayerMove()   //�v���C���[�̈ړ�
    {
        moveVec = shotPos - trans.position; //�ړ������Z�o

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

    private void PlayerHitJudge()   //�v���C���[���I�u�W�F�N�g�ɓ������Ă��邩
    {
        Ray ray = new Ray(playerTrans.position, moveVec);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, hitRange))
        {
            hitFlg = true;
        }
    }

    private void Relieve()   //��������
    {
        metalFlg = false;
        moveFinishFlg = false;
        hitFlg = false;
        return;
    }
}
