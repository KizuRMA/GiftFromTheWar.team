using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class batMove : BaseState
{
    [SerializeField] GameObject playerCC;
    [SerializeField] bool moveFlg;
    [SerializeField] float playerFromInterval;
    [SerializeField] float ultrasoundCoolTime;
    private NavMeshAgent agent;
    private UltraSoundBeam ultrasound;
    private float untilLaunch;

    // Start is called before the first frame update
    public override void Start()
    {
        ultrasound = null;
        myController = GetComponent<BatController>();
        agent = GetComponent<NavMeshAgent>();
        playerCC = GameObject.Find("player").gameObject;

        //�����g��������
        ultrasound = GetComponent<UltraSoundBeam>();
        ultrasound.Init();

        untilLaunch = Time.time; ;

        myController.OnNavMesh();
    }

    // Update is called once per frame
    public override void Update()
    {
        //�̂�O�ɌX����
        Vector3 _localAngle = transform.localEulerAngles;
        _localAngle.x = myController.forwardAngle;
        transform.localEulerAngles = _localAngle;

        //�����𒲐�����
        myController.AdjustHeight();

        //Debug.Log(transform.position);

        //�ړ�����ꍇ
        if (moveFlg)
        {
            Vector3 _playerPos = playerCC.transform.position;
            Vector3 _myPos = transform.position;

            //�v���C���[�ɋ߂Â������Ȃ�����
            float dis = Vector3.Distance(_myPos, _playerPos);

            //�v���C���[�Ƃ̋����𒲂ׂ�
            if (dis <= playerFromInterval)
            {
                //�߂Â������Ă���ꍇ
                agent.destination = _myPos;
                BatController batCon = gameObject.GetComponent<BatController>();

                batCon.ChangeState(GetComponent<WingFoldState>());
                //�������^�[��
                return;
            }
            else
            {
                if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
                {
                    //����Ă���ꍇ
                    agent.destination = _playerPos;
                }

            }

            //�����g����
            if (ultrasound != null && untilLaunch + ultrasoundCoolTime <= Time.time)
            {
                ultrasound.Update();
            }

            //�����g���o���؂����ꍇ
            if (ultrasound.IsAlive() == false)
            {
                //�����g������������
                ultrasound.Init();
                untilLaunch = Time.time;
            }
        }
    }
}
