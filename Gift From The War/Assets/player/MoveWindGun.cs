using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWindGun : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    private CharacterController CC;
    private Transform trans;
    [SerializeField] private GameObject cam;
    [SerializeField] private FPSController fpsCon;
    [SerializeField] private playerHundLadder ladder;
    [SerializeField] private remainingAmount energyAmount;
    [SerializeField] private playerDied died;

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

    // Start is called before the first frame update
    void Start()
    {
        CC = this.GetComponent<CharacterController>();
        trans = transform;
        upWindFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (died.diedFlg) return;
        if (ladder.touchLadderFlg) return;

        KnowViewpoint();

        Move();
    }

    private void KnowViewpoint() //�ǂ��ނ��Ă��邩
    {
        if (!Input.GetMouseButton(0)) return;  //�N���b�N���Ă��Ȃ������珈�����s��Ȃ�
        if (energyAmount.GetSetNowAmount <= 0) return;  //�G�l���M�[�̎c�ʂ��Ȃ������珈�����s��Ȃ�

        viewpoint = Quaternion.Euler(cam.transform.localRotation.eulerAngles.x, trans.localRotation.eulerAngles.y, 0);  //�����Ă�������v�Z
    }

    private void Move() //�ړ��̏���
    {
        if (!Input.GetMouseButton(0) || energyAmount.GetSetNowAmount <= 0)   //�N���b�N���Ă��Ȃ�������A�܂��̓G�l���M�[�c�ʂ��Ȃ�������
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

        if (fpsCon.groundFlg)  //�ݒn����
        {
            groundFlg = true;
        }

        if (groundFlg)  //��C��R�̏���
        {
            airResistance -= airResistancePower * Time.deltaTime;
        }

        if (airResistance < 0)   //��C��R�I��
        {
            airResistance = 0;
            groundFlg = false;
        }

        //��C��R�̈ړ��ʂ̌v�Z
        power = viewpoint * new Vector3(0, 0, -movePower) * airResistance * Time.deltaTime;
        power.y = 0;
        CC.Move(power);

        energyAmount.GetSetNowAmount = 0;   //�G�l���M�[�����
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

        energyAmount.GetSetNowAmount = useEnergyAmount; //�G�l���M�[����
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
}