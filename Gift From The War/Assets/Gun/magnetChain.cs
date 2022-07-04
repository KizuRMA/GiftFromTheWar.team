using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnetChain : ShootParent
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    [SerializeField] private Transform playerTrans;
    [SerializeField] private GameObject bulletLineEffect;
    private GameObject bulletLinePos;
    private bulletChange bulletChange;
    private magnet magnet;

    //�v���C���[�֌W�̃p�����[�^
    private CharacterController CC;
    private GetItem getItem;
    private Gravity gravity;

    //�e�̔���
    private bool shotFlg;                       //���ˉ\

    //�ړ�����
    public bool metalFlg { get; set; }              //�����ɂ��������t���O
    [SerializeField] private float moveSpeed;       //�ړ����鑬��
    [SerializeField] private float moveSpeedMax;    //�ړ����鑬���̍ő�l
    private float nowMoveSpeed;                     //���̈ړ��X�s�[�h
    private Vector3 moveVec;                        //�ړ�����
    private bool moveFinishFlg = false;             //�ړ����I������t���O
    private bool hitFlg = false;                    //�I�u�W�F�N�g�ɂ���������
    private Vector3 prePos;                         //�O�t���[���̈ʒu���L�����Ă���
    private bool useEnergy0 = false;                //�G�l���M�[����ʂ�0�ɂ���t���O
    [SerializeField] private float hitRange;        //�����蔻��͈̔�
    private float nowTime = 0;                      //���̈ړ�����
    [SerializeField] private float timeMax;         //�ړ����Ԃ̌��E�l

    private void Start()
    {
        CC = playerTrans.GetComponent<CharacterController>();
        gravity = playerTrans.GetComponent<Gravity>();
        getItem = playerTrans.GetComponent<GetItem>();

         //�ϐ�������
         bulletLinePos = transform.Find("muzzlePosLine").gameObject;

        if (transform.parent != null)
        {
            bulletChange = transform.parent.GetComponent<bulletChange>();
        }

        magnet = transform.GetComponent<magnet>();

        trans = transform;
        metalFlg = false;
        bulletLineEffect.SetActive(false);
    }

    public void Finish()    //�����؂�ւ������̏I������
    {
        bulletLineEffect.SetActive(false);
        shotFlg = false;
        metalFlg = false;
        moveFinishFlg = false;
        hitFlg = false;
        useEnergy0 = false;
    }

    void Update()
    {
        if (!getItem.magnetAmmunitionFlg) return;   //�e���E���ĂȂ������珈�����Ȃ�

        if (magnet.metal != null) return;   //���łɕʂ̎��΂�ł��Ă����珈�����Ȃ�

        MoveBullet();

        if (bulletChange.nowBulletType != bulletChange.bulletType.e_magnet) return; //���̒e�̎�ނ��Ή����ĂȂ�������

        //�G�l���M�[���K�v�ʂ����
        shotFlg = energyAmount.GetSetNowAmount > (1.0f - useEnergy);

        if (useEnergy0)  //�G�l���M�[����ʂ�0�ɂ���
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
        if (!metalFlg)
        {
            bulletLineEffect.SetActive(false);
            return;
        }

        MagnetChain();
    }

    private void Shot() //�e��ł���
    {
        energyAmount.GetSetNowAmount = useEnergy;
        energyAmount.useDeltaTime = false;

        nowTime = 0;

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

    private void MagnetChain()   //�����̕��ɔ��
    {
        prePos = playerTrans.position;
        PlayerMove();
        PlayerHitJudge();

        //���Ԃ̏���
        nowTime += Time.deltaTime;

        //�������鏈��
        if (moveFinishFlg || hitFlg || nowTime > timeMax)
        {
            Relieve();
        }
    }

    private void PlayerMove()   //�v���C���[�̈ړ�
    {
        moveVec = shotPos - trans.position; //�ړ������Z�o
        moveVec.y = gravity.groundHitFlg && moveVec.y < 0 ? 0 : moveVec.y;

        //���C���G�t�F�N�g
        bulletLineEffect.SetActive(true);
        bulletLineEffect.transform.position = bulletLinePos.transform.position;
        bulletLineEffect.transform.LookAt(shotPos);

        //��������
        nowMoveSpeed += (nowMoveSpeed + moveSpeed) * (nowMoveSpeed + moveSpeed) * Time.deltaTime;
        nowMoveSpeed = nowMoveSpeed > moveSpeedMax ? moveSpeedMax : nowMoveSpeed;   //�ő�l���傫��������A�ő�l�ɂ���

        if (Mathf.Abs(moveVec.magnitude) > nowMoveSpeed * Time.deltaTime)  //�ړ��ʂ��傫��������A���ɂ���
        {
            moveVec = moveVec.normalized * nowMoveSpeed * Time.deltaTime;
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
        nowMoveSpeed = 0;
        metalFlg = false;
        moveFinishFlg = false;
        hitFlg = false;
        return;
    }
}
