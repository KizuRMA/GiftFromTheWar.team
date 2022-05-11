using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class batMove : BaseState
{
    private CharacterController playerCC;
    private CharacterController batCC;
    private NavMeshAgent agent;
    private UltraSoundBeam ultrasound;
    Transform defaltTransform;
    [SerializeField] bool moveFlg;
    [SerializeField] float playerFromInterval;
    [SerializeField] float ultrasoundCoolTime;
    private float untilLaunch;

    // Start is called before the first frame update
    public override void Start()
    {
        ultrasound = null;
        playerCC = GameObject.Find("player").GetComponent<CharacterController>();
        batCC = GetComponent<CharacterController>();
        myController = GetComponent<BatController>();
        agent = GetComponent<NavMeshAgent>();
        defaltTransform = GetComponent<Transform>();

        //�����g��������
        ultrasound = GetComponent<UltraSoundBeam>();
        ultrasound.Init();

        untilLaunch = Time.time; ;

        agent.isStopped = false;
        agent.updateUpAxis = true;
        agent.updateRotation = true;
        agent.updatePosition = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        //�̂�O�ɌX����
        Vector3 _localAngle = transform.localEulerAngles;
        _localAngle.x = myController.forwardAngle;
        transform.localEulerAngles = _localAngle;

        //�����𒲐�����
        Ray _ray = new Ray(transform.position, Vector3.up);
        RaycastHit _raycastHit;
        bool _hit = Physics.Raycast(_ray, out _raycastHit);

        //�X�e�[�W�̗������L�^
        float _hight = _raycastHit.distance;

        //�X�e�[�W�̏c���̂S���̈ʒu�ɂ���悤�ɂ���
        _hight *= 0.4f;
        //�R�E�����̔�s�����ݒ肷��
        if (_hight > 0.8f)
        {
            _hight = 0.8f;
        }

        //���݂̃R�E�������������܂񂾍��W
        Vector3 nowPos = new Vector3(transform.position.x,myController.hight, transform.position.z);
        //�{�����Ăق������W
        Vector3 nextPos = new Vector3(transform.position.x,_hight, transform.position.z);

        //�i�r���b�V���̃X�s�[�h��p���ăR�E�����̍����𒲐�����
        nowPos = Vector3.MoveTowards(nowPos, nextPos, 0.001f);

        //���̃t���[���ł͌��݂�Y�����ۑ�����Ȃ����߁A�L�^���Ă����B
        myController.hight = nowPos.y;

        transform.position = new Vector3(transform.position.x,transform.position.y + myController.hight,transform.position.z);

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
                //����Ă���ꍇ
                agent.destination = _playerPos;
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

        if (Input.GetKey(KeyCode.E))
        {
            BatController batCon = gameObject.GetComponent<BatController>();
            batCon.ChangeState(GetComponent<DeadState>());
            //�������^�[��
            return;
        }
    }
}
