using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWindGun : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    private CharacterController CC;
    private Transform trans;
    [SerializeField] private GameObject cam;
    private Gravity gravity;
    private playerHundLadder ladder;
    private remainingAmount energyAmount;
    private playerDied died;
    private bulletChange bulletChange;
    private GetItem getItem;
    private shooting shoot;
    private PlayerStartDown playerStartDown;

    //�ړ�
    [SerializeField] private float movePower;
    [SerializeField] private float movePowerMin;    //�p���[�̍Œ�l
    [SerializeField] private float range;           //��C���͂��˒�
    private float disRaitoPower;                    //�����ɂ��␳
    [SerializeField] private float useEnergyAmount; //����G�l���M�[��
    private Quaternion viewpoint;                   //�����Ă����
    private Vector3 power;                          //�ŏI�I�Ȉړ���

    //��C��R
    private bool groundFlg = false;                     //�n�ʂɂ��Ă��邩
    private float airResistance;                        //���݂̋�C��R
    [SerializeField] private float airResistancePower;  //��C��R��
    [SerializeField] private float airResistanceMax;    //��C��R�̍ő�l
    [SerializeField] private float airResistanceMin;    //��C��R�̍ŏ��l

    public bool upWindFlg { get; set; }
    public bool effectFlg { get; set; }

    void Start()
    {
        //�ϐ���������
        GunUseInfo _info = transform.GetComponent<GunUseInfo>();

        gravity = transform.GetComponent<Gravity>();
        ladder = transform.GetComponent<playerHundLadder>();
        energyAmount = _info.cube.GetComponent<remainingAmount>();
        died = transform.GetComponent<playerDied>();
        bulletChange = _info.gunModel.GetComponent<bulletChange>();
        getItem = transform.GetComponent<GetItem>();
        shoot = _info.muzzlePos.GetComponent<shooting>();
        playerStartDown = transform.GetComponent<PlayerStartDown>();

        CC = this.GetComponent<CharacterController>();
        trans = transform;
        upWindFlg = false;
        effectFlg = false;
    }

    public void Finish()    //�����؂�ւ������̏I������
    {
        upWindFlg = false;
        effectFlg = false;
    }

    void Update()
    {
        if (!getItem.windAmmunitionFlg) return; //�e���E���ĂȂ������珈�����Ȃ�
        if (playerStartDown != null && playerStartDown.isAuto == true) return;

        if (bulletChange.nowBulletType != bulletChange.bulletType.e_wind || bulletChange.cylinder.isChanging == true) return;   //���̒e�̎�ނ��Ή����ĂȂ�������

        if (died.diedFlg) return;
        if (ladder.touchLadderFlg) return;

        KnowViewpoint();

        Move();
    }

    private void KnowViewpoint() //�ǂ��ނ��Ă��邩
    {
        effectFlg = false;
        if (!Input.GetMouseButton(0)) return;  //�N���b�N���Ă��Ȃ������珈�����s��Ȃ�
        if (energyAmount.GetSetNowAmount <= 0) return;  //�G�l���M�[�̎c�ʂ��Ȃ������珈�����s��Ȃ�

        viewpoint = Quaternion.Euler(cam.transform.localRotation.eulerAngles.x, trans.localRotation.eulerAngles.y, 0);  //�����Ă�������v�Z
        effectFlg = true;
        AudioManager.Instance.PlaySE("Wind", isLoop: false);

    }

    private void Move() //�ړ��̏���
    {
        if ((!Input.GetMouseButton(0)) || energyAmount.GetSetNowAmount <= 0)   //�N���b�N���Ă��Ȃ�������A�܂��̓G�l���M�[�c�ʂ��Ȃ�������
        {
            AirResistance();
            return;
        }

        if (energyAmount.GetSetNowAmount <= 0) return;

        WindMove();
    }

    private void AirResistance()    //��C��R����
    {
        upWindFlg = false;

        if (gravity.firstGroundHitFlg)  //�ݒn����
        {
            groundFlg = true;
        }

        if (groundFlg)  //��C��R�̏���
        {
            airResistance -= airResistancePower * Time.deltaTime;
        }

        airResistance -= airResistancePower * Time.deltaTime;

        if (airResistance < 0)   //��C��R�I��
        {
            airResistance = 0;
            groundFlg = false;
        }

        //��C��R�̈ړ��ʂ̌v�Z
        power = viewpoint * new Vector3(0, 0, -movePower) * airResistance * Time.deltaTime;
        power.y = 0;
        CC.Move(power);

        if (shoot.shotFlg) return;  //�e�𔭎˂��Ă�����A�G�l���M�[����ʂ�0�ɂ��Ȃ�
        energyAmount.GetSetNowAmount = 0;   //�G�l���M�[����ʂ�0�ɂ���
    }

    private void WindMove() //���̈ړ��̏���
    {
        upWindFlg = true;

        CorrectionDis();

        //���̈ړ��ʂ̌v�Z
        power = viewpoint * new Vector3(0, 0, -movePower) * disRaitoPower * Time.deltaTime;
        CC.Move(power);

        //�e�̌��������������Ă��Ȃ�������A��ɕ����͂��I�t����
        if (cam.transform.localRotation.eulerAngles.x < 40)
        {
            upWindFlg = false;
        }

        //�G�l���M�[����
        energyAmount.GetSetNowAmount = useEnergyAmount;
        energyAmount.useDeltaTime = true;
    }

    private void CorrectionDis()    //�n�ʂ���ǂꂾ������Ă��邩
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);   //�v���C���[�̉��Ƀ��C������΂�
        RaycastHit hit;
        int layerMask = 1 << 9;
        if (Physics.Raycast(ray, out hit, range, layerMask))    //�n�ʂ����苗�����ɂ�����
        {
            disRaitoPower = 1.0f - hit.distance / range + movePowerMin; //����Ă��鋗���ɉ����āA���̋������ς��

            //��C��R�̌v�Z�̂Ƃ��̂��߂ɁA�l��ێ�����
            airResistance = disRaitoPower;
            if (airResistance < airResistanceMin)
            {
                airResistance = airResistanceMin;
            }

            if(airResistance > airResistanceMax)
            {
                airResistance = airResistanceMax;
            }
        }
        else
        {
            disRaitoPower = 0;
        }
    }

    private void ObjectsAffect()    //���ɂ��I�u�W�F�N�g�ւ̉e��
    {
        //GameObject stage = GameObject.FindGameObjectWithTag("stage");
        //if (stage == null) return;

        //List<GameObject> gameList = new List<GameObject>();

        //GameObject game;
        //game = stage.transform.Find("dynamicObj").gameObject;

        ////�v���C���[�Ƌ߂��X�e�[�W�I�u�W�F�N�g�����X�g�ɕۊ�
        //for (int i  = 0; i < game.transform.childCount; i++)
        //{
        //    for (int j = 0; j < game.transform.GetChild(i).childCount; j++)
        //    {
        //        Transform trans = game.transform.GetChild(i).GetChild(j).transform;
        //        float dis = Vector3.Distance(trans.position,CC.transform.position);

        //        if (dis < range)
        //        {
        //            gameList.Add(game.transform.GetChild(i).GetChild(j).gameObject);
        //        }
        //    }
        //}

        //if (gameList.Count == 0) return;

        //for (int i = 0; i < gameList.Count; i++)
        //{
        //    //�^�[�Q�b�g�ւ̃x�N�g��
        //    //Vector3 _targetVec = gameList[i].transform.position - CC.transform.position;
        //    //float dis

        //}

    }
}