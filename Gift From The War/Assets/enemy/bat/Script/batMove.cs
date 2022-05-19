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
    private UltraSoundBeam ultrasoundBeam;
    private UltraSound ultrasound;
    private float untilLaunch;
    private e_Action nowAction;


    // Start is called before the first frame update
    public override void Start()
    {
        myController = GetComponent<BatController>();
        agent = GetComponent<NavMeshAgent>();
        playerCC = GameObject.Find("player").gameObject;

        //�����g��������
        ultrasoundBeam = GetComponent<UltraSoundBeam>();
        ultrasoundBeam.Init();

        ultrasound = GetComponent<UltraSound>();
        ultrasound.Init();

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

        //��l���̍��W��������Ȃ��ꍇ
        if (abnormalcondition.IsHowling() == false)
        {
            ultrasoundBeam.Init();

            if (agent.velocity.magnitude <= 0.0f)
            {
                nowAction = e_Action.search;
            }
        }
        else
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
                if (ultrasoundBeam != null && untilLaunch - ultrasoundCoolTime > 0)
                {
                    bool hit = false;
                    hit = ultrasoundBeam.Update();
                    if (hit == true)
                    {
                        //�v���C���[�Ƀn�E�����O��Ԃ�t������
                        abnormalcondition.AddHowlingAbnormal();
                    }
                }

                //�����g���o���؂����ꍇ
                if (ultrasoundBeam.IsAlive() == false)
                {
                    //�����g������������
                    ultrasoundBeam.Init();
                    untilLaunch = 0;
                }

            }
        }
    }

    private void ActionSearch()
    {
        nowAction = e_Action.check;
    }

    private void ActionCheck()
    {
        bool hit = false;
        hit = ultrasound.Update();

        if (hit == true)
        {
            ultrasound.Init();
            untilLaunch = 0;

            //�v���C���[�Ƀn�E�����O��Ԃ�t������
            playerAbnormalcondition abnormalcondition = playerCC.GetComponent<playerAbnormalcondition>();
            abnormalcondition.AddHowlingAbnormal();
            nowAction = e_Action.move;
            return;
        }

        //�����g���o���؂����ꍇ
        if (ultrasound.IsAlive() == false)
        {
            ultrasound.Init();
            untilLaunch = 0;

            BatController batCon = gameObject.GetComponent<BatController>();
            batCon.ChangeState(GetComponent<WingFoldState>());
            //�������^�[��
            return;
        }
    }
}
