using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWindGun : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    private CharacterController CC;
    private Transform trans;
    [SerializeField] private GameObject cam;
    [SerializeField] private playerHundLadder ladder;
    [SerializeField] private remainingAmount energyAmount;

    //�ړ�
    [SerializeField] private float movePower;
    [SerializeField] private float movePowerMin;    //�p���[�̍Œ�l
    [SerializeField] private float range;           //��C���͂��˒�
    private float disRaitoPower;                    //�����ɂ��␳
    [SerializeField] private float useEnergyAmount; //����G�l���M�[��
    private Quaternion viewpoint;                   //�����Ă����
    private Vector3 power;                          //�ŏI�I�Ȉړ���
    private float gravity;                          //�d�̗͂�

    //��C��R
    private bool groundFlg = false;                     //�n�ʂɂ��Ă��邩
    private float airResistance;                        //���݂̋�C��R
    [SerializeField] private float airResistancePower;  //��C��R��
    [SerializeField] private float airResistanceMax;    //��C��R�̍ő�l
    [SerializeField] private float airResistanceMin;    //��C��R�̍ŏ��l

    private bool upWindFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        CC = this.GetComponent<CharacterController>();
        trans = transform;
        gravity = this.GetComponent<FPSController>().GetGravity;
    }

    // Update is called once per frame
    void Update()
    {
        if (ladder.GetTouchLadderFlg()) return;

        KnowViewpoint();

        Move();
    }

    private void KnowViewpoint() //�ǂ��ނ��Ă��邩
    {
        if (!Input.GetMouseButton(0)) return;  //�N���b�N���Ă��Ȃ������珈�����s��Ȃ�
        if (energyAmount.GetSetNowAmount <= 0) return;  //�G�l���M�[�̎c�ʂ��Ȃ������珈�����s��Ȃ�

        viewpoint = Quaternion.Euler(cam.transform.localRotation.eulerAngles.x, trans.localRotation.eulerAngles.y, 0);  //�����Ă�������v�Z

        energyAmount.GetSetNowAmount = useEnergyAmount; //�G�l���M�[����
    }

    private void Move() //�ړ��̏���
    {
        if (!Input.GetMouseButton(0))
        {
            upWindFlg = false;

            if (CC.isGrounded)
            {
                groundFlg = true;
            }

            if (groundFlg)
            {
                airResistance -= airResistancePower * Time.deltaTime;
            }

            if(airResistance < 0)
            {
                airResistance = 0;
                groundFlg = false;
            }

            power = viewpoint * new Vector3(0, 0, -movePower) * airResistance * Time.deltaTime;
            power.y = 0;
            CC.Move(power);

            energyAmount.GetSetNowAmount = 0;

            return;
        }

        if (energyAmount.GetSetNowAmount <= 0) return;

        upWindFlg = true;
        CorrectionDis();

        power = viewpoint * new Vector3(0, 0, -movePower) * disRaitoPower * Time.deltaTime;

        if (cam.transform.localRotation.eulerAngles.x < 40)
        {
            upWindFlg = false;
        }
        CC.Move(power);
    }

    private void CorrectionDis()    //�n�ʂ���ǂꂾ������Ă��邩
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        int layerMask = 1 << 9;
        if (Physics.Raycast(ray, out hit, range, layerMask))
        {
            disRaitoPower = 1.0f - hit.distance / range + movePowerMin;

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

    public bool GetUpWindGunFlg()
    {
        return upWindFlg;
    }

    public Vector3 GetPower()
    {
        return power;
    }
}