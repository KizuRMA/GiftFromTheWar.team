using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class batMove : BaseState
{
    enum e_Action
    {
        move,
        search,
        check,
    }

    [SerializeField] GameObject playerCC;
    [SerializeField] bool moveFlg;
    [SerializeField] float playerFromInterval;
    [SerializeField] float ultrasoundCoolTime;
    private NavMeshAgent agent;
    private float untilLaunch;
    private e_Action nowAction;


    // Start is called before the first frame update
    public override void Start()
    {
        myController = GetComponent<BatController>();
        agent = GetComponent<NavMeshAgent>();
        playerCC = GameObject.Find("player").gameObject;

        //�����g��������
        ChageUltrasound(GetComponent<UltraSoundBeam>());
        ultrasoundCoolTime = ultrasound.coolDown;

        CurrentState = (int)BatController.e_State.move;
        nowAction = e_Action.move;
        untilLaunch = 0;

        myController.OnNavMesh();
    }

    // Update is called once per frame
    public override void Update()
    {
        untilLaunch += Time.deltaTime;

        //�̂�O�ɌX����
        Vector3 _localAngle = transform.localEulerAngles;
        _localAngle.x = myController.forwardAngle;
        transform.localEulerAngles = _localAngle;

        //�����𒲐�����
        myController.AdjustHeight();

        switch (nowAction)
        {
            case e_Action.move:
                ActionMove();
                break;
            case e_Action.search:
                ActionSearch();
                break;
            case e_Action.check:
                ActionCheck();
                break;
        }
    }

    private void ActionMove()
    {
        playerAbnormalcondition abnormalcondition = playerCC.GetComponent<playerAbnormalcondition>();

        //��l�����n�E�����O��Ԃ̎�
        if (abnormalcondition.IsHowling() == true)
        {
            //�ړ�����ꍇ
            if (moveFlg)
            {
                Vector3 _playerPos = playerCC.transform.position;
                Vector3 _myPos = transform.position;

                if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
                {
                    //����Ă���ꍇ
                    agent.destination = _playerPos;
                }

                //�����g����
                if (ultrasound != null && untilLaunch - ultrasoundCoolTime > 0)
                {
                    bool hit = false;
                    ultrasound.Update();
                    ultrasound.DrawLine();
                    hit = ultrasound.CheckHit();
                    if (hit == true)
                    {
                        //�v���C���[�Ƀn�E�����O��Ԃ�t������
                        abnormalcondition.AddHowlingAbnormal();
                    }
                }

                //�����g���o���؂����ꍇ
                if (ultrasound.IsAlive == false)
                {
                    //�����g������������
                    ultrasound.Init();
                    untilLaunch = 0;
                }

            }
        }
        else
        {
            ultrasound.Init();

            if (agent.velocity.magnitude <= 0.0f)
            {
                //�A�N�V������Ԃ��T�[�`��Ԃɕω�
                nowAction = e_Action.search;
                Animator animator = GetComponent<Animator>();
                animator.SetTrigger("ShakeHead");

                ChageUltrasound(GetComponent<SmallUltrasound>());
            }
        }
    }

    private void ActionSearch()
    {
        ultrasound.Update();
        ultrasound.DrawLine();
        bool hit = ultrasound.CheckHit();
        if (hit == true)
        {
            playerAbnormalcondition abnormalcondition = playerCC.GetComponent<playerAbnormalcondition>();
            abnormalcondition.AddHowlingAbnormal();
        }
    }

    private void ActionCheck()
    {
        playerAbnormalcondition abnormalcondition = playerCC.GetComponent<playerAbnormalcondition>();

        bool hit = false;
        ultrasound.Update();
        ultrasound.DrawLine();
        hit = ultrasound.CheckHit();

        if (hit == true || abnormalcondition.IsHowling() == true)
        {
            ultrasound.Init();
            untilLaunch = 0;

            //�v���C���[�Ƀn�E�����O��Ԃ�t������
            abnormalcondition.AddHowlingAbnormal();
            nowAction = e_Action.move;
            return;
        }

        //�����g���o���؂����ꍇ
        if (ultrasound.IsAlive == false)
        {
            ultrasound.Init();
            untilLaunch = 0;

            BatController batCon = gameObject.GetComponent<BatController>();
            batCon.ChangeState(GetComponent<WingFoldState>());
            //�������^�[��
            return;
        }
    }

    public void SearchPlayerAction()
    {
        nowAction = e_Action.check;
        ChageUltrasound(GetComponent<LargeUltrasound>());
        ultrasoundCoolTime = ultrasound.coolDown;
    }

}
