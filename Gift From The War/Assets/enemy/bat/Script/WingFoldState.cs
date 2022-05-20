using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WingFoldState : BaseState
{
    enum e_Action
    {
        none,
        search,
        move,
        sticking,
        leave,
    }

    [SerializeField] float ultrasoundCoolTime;
    [SerializeField] float ascendingSpeed;
    private Vector3 targetPos;
    private RaycastHit hit;
    private CharacterController playerCC;
    private NavMeshAgent agent;
    private UltraSound ultrasound;
    private GameObject childGameObject;
    private bool nextAnime;
    private bool navmeshOnFlg;
    private float frame;
    private float distance;
    private float defaltHight;
    private e_Action nowAction;
    private float rotateY;
    private float untilLaunch;
    private float amountChangeAngX;
    private float amountChangeDis;

    // Start is called before the first frame update
    public override void Start()
    {
        rotateY = transform.eulerAngles.y;
        nextAnime = false;
        navmeshOnFlg = true;
        nowAction = e_Action.search;
        untilLaunch = 0;
        distance = 0;
        defaltHight = 0;
        frame = 20;

        myController = GetComponent<BatController>();
        agent = GetComponent<NavMeshAgent>();
        playerCC = GameObject.Find("player").GetComponent<CharacterController>();
        childGameObject = transform.Find("Capsule").gameObject;
        childGameObject.GetComponent<CapsuleCollider>().enabled = true;
        childGameObject.GetComponent<BatCapsuleScript>().Start();

        ultrasound = GetComponent<UltraSound>();
        ultrasound.Init();

        CurrentState = (int)BatController.e_State.wingFold;
    }

    // Update is called once per frame
    public override void Update()
    {
        bool _navmeshFlg = navmeshOnFlg;

        if (agent.isStopped == true)
        {
            //�̂���]�����鏈��
            if (myController.forwardAngle >= 90)
            {
                transform.eulerAngles = new Vector3(180.0f - myController.forwardAngle, rotateY + 180.0f, 180.0f);
            }
            else
            {
                transform.eulerAngles = new Vector3(myController.forwardAngle, rotateY, 0);
            }
        }
        else
        {
            myController.SimpleAdjustHeight();
        }

        //���݂̃A�N�V������Ԗ��Ɋ֐������s����
        switch (nowAction)
        {
            //����t�������
            case e_Action.none:
                ActionNone();
                break;
            //����t���ꏊ��T��
            case e_Action.search:
                ActionSearch();
                break;
            case e_Action.move:
                ActionMove();
                break;
            //����t���ɂ�����Ԃ̎�
            case e_Action.sticking:
                ActionSticking();
                break;
            //������Ԃ̎�
            case e_Action.leave:
                ActionLeave();
                break;
        }

        if (agent.isStopped == true)
        {
            //�R�E�����̍��W���烏�[���h���W�ŉ��Ɍ����Ă��郌�C�����
            int layerMask = 1 << 9;
            Ray _ray = new Ray(transform.position, Vector3.down);
            RaycastHit _raycastHit;

            bool _rayHit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, layerMask);

            Debug.Log(myController.hight);

            //���C���I�u�W�F�N�g�ɓ������Ă���ꍇ
            if (_rayHit == true)
            {
                //���݂̍������L�^���Ă���
                myController.hight = _raycastHit.distance;
            }
        }

        //�i�r���b�V���ɕύX���������Ă���ꍇ
        if (_navmeshFlg != navmeshOnFlg)
        {
            if (navmeshOnFlg == true)
            {
                myController.OnNavMesh();
            }
            else
            {
                myController.OffNavMesh();
            }
        }
    }

    private void ActionSticking()
    {
        Animator animator = GetComponent<Animator>();

        //�^�[�Q�b�g�Ƃ��Ă�����W�܂ł̋����𒲂ׂ�
        float _targetDis = Vector3.Distance(transform.position, targetPos);

        //�V��Ƃ̋������ړ��ʂ����傫���A�܂��͋t���܂ɂȂ��Ă��Ȃ��ꍇ
        if (_targetDis >= ascendingSpeed || myController.forwardAngle < 180.0f)
        {
            //��Ɉړ����鏈��
            transform.position = Vector3.MoveTowards(transform.position, targetPos, ascendingSpeed * Time.deltaTime);
            _targetDis = Vector3.Distance(transform.position, targetPos);
        }
        else
        {
            //�A�N�V������Ԃ�ύX����
            nowAction = e_Action.none;
            untilLaunch = 0;
            nextAnime = false;
            return;
        }

        //�V��Ƃ̍������߂��ꍇ
        if (_targetDis <= 0.5f)
        {
            GameObject.Find("CollisionDetector").GetComponent<BoxCollider>().enabled = false;

            //�R�E������180�x��]���Ă��Ȃ��ꍇ
            if (myController.forwardAngle < 180.0f)
            {
                myController.forwardAngle += 1.0f;

                if (myController.forwardAngle >= 180.0f)
                {
                    myController.forwardAngle = 180.0f;
                }
            }

            if (nextAnime == false)
            {
                //�H�����A�j���[�V�����ɐ؂�ւ���
                animator.SetInteger("trans", 1);
                nextAnime = true;
            }
        }
    }

    private void ActionNone()
    {
        untilLaunch += Time.deltaTime;

        //�����g�̃N�[���^�C�����I�����Ă���ꍇ
        if (untilLaunch - ultrasoundCoolTime > 0)
        {
            //�����g���X�V
            bool _hit = ultrasound.Update();

            //�����g���v���C���[�ɓ������Ă���ꍇ
            if (_hit == true)
            {
                //�A�N�V������Ԃ�V�䂩�痣����Ԃɕω�
                nowAction = e_Action.leave;
                targetPos += Vector3.down * 0.8f;
                amountChangeDis = Vector3.Distance(targetPos, transform.position);
                amountChangeAngX = myController.forwardAngle - 20.0f;

                //�v���C���[���n�E�����O��Ԃɂ���
                playerCC.GetComponent<playerAbnormalcondition>().AddHowlingAbnormal();

                //�A�j���[�V������؂�ւ���
                Animator animator = GetComponent<Animator>();
                animator.SetInteger("trans", 2);
                ultrasound.Init();
                return;
            }
        }

        //�����g���o���؂����ꍇ
        if (ultrasound.IsAlive() == false)
        {
            ultrasound.Init();
            untilLaunch = 0;
        }
    }

    private void ActionSearch()
    {
        //�J�v�Z���R���C�_�[�̏�������~���Ă���ꍇ�͊J�n����
        CapsuleCollider capsule = childGameObject.GetComponent<CapsuleCollider>();
        if (capsule.enabled == false) capsule.enabled = true;

        //�w��̃t���[����������
        frame--;
        if (frame > 0) return;

        BatCapsuleScript _batCapsule = childGameObject.GetComponent<BatCapsuleScript>();

        if (_batCapsule.colList.Count <= 0)
        {
            nowAction = e_Action.sticking;
            navmeshOnFlg = false;
            targetPos = SearchCeiling(transform.position);
        }
        else
        {
            //��Q��������ł������������o��
            Vector3 _targetVec = _batCapsule.MoveDirction();
            _targetVec.Normalize();
            _targetVec *= 2.0f;
            _targetVec += transform.position;

            //�i�r���b�V���Ȃ��ɕ⊮����
            NavMeshHit hit;
            if (NavMesh.SamplePosition(_targetVec, out hit, 1.0f, NavMesh.AllAreas))
            {
                // �ʒu��NavMesh���ɕ␳
                _targetVec = hit.position;
            }

            distance = Vector3.Distance(_targetVec, transform.position);
            defaltHight = myController.hight;
            targetPos = SearchCeiling(_targetVec);

            agent.destination = _targetVec;
            nowAction = e_Action.move;
        }
    }

    private void ActionMove()
    {
        BatCapsuleScript _batCapsule = childGameObject.GetComponent<BatCapsuleScript>();

        Vector3 _myPos = transform.position;
        Vector3 _targetPos = agent.destination;
        float _maxHight = Vector3.Distance(_targetPos, targetPos);

        _myPos.y = 0;
        _targetPos.y = 0;

        float _targetDistance = Vector3.Distance(_myPos, _targetPos);
        float _amountMove = Mathf.Abs(distance - _targetDistance);

        myController.hight = NeedHight(_amountMove, defaltHight, _maxHight, distance);

        if (_targetDistance <= 0.1f)
        {
            Animator animator = GetComponent<Animator>();

            //�R�E������180�x��]���Ă��Ȃ��ꍇ
            if (myController.forwardAngle < 180.0f)
            {
                myController.forwardAngle += 1.0f;

                if (myController.forwardAngle >= 180.0f)
                {
                    myController.forwardAngle = 180.0f;
                }
            }

            //�̂���]�����鏈��
            if (myController.forwardAngle >= 90)
            {
                transform.eulerAngles = new Vector3(180.0f - myController.forwardAngle, rotateY + 180.0f, 180.0f);
            }
            else
            {
                transform.eulerAngles = new Vector3(myController.forwardAngle, rotateY, 0);
            }

            if (nextAnime == false)
            {
                //�H�����A�j���[�V�����ɐ؂�ւ���
                animator.SetInteger("trans", 1);
                nextAnime = true;
            }

            childGameObject.GetComponent<CapsuleCollider>().enabled = false;

            if (_targetDistance <= 0.001f)
            {
                navmeshOnFlg = false;
                transform.position = targetPos;
                nowAction = e_Action.none;
            }
        }
        else
        {
            rotateY = transform.eulerAngles.y;
            //�̂�O�ɌX����
            Vector3 _localAngle = transform.localEulerAngles;
            _localAngle.x = myController.forwardAngle;
            transform.localEulerAngles = _localAngle;
        }
    }

    private void ActionLeave()
    {
        //�����L����A�j���[�V�����ɕύX
        Animator animator = GetComponent<Animator>();

        //�R�E�����ړ�����
        transform.position = Vector3.MoveTowards(transform.position, targetPos, amountChangeDis * (Time.deltaTime * 1.5f));

        //�R�E�����̉�]����
        if (myController.forwardAngle > 20.0f)
        {
            //�ω�����l
            float _changeAng = amountChangeAngX * (Time.deltaTime * 1.5f);
            myController.forwardAngle -= _changeAng;

            if (myController.forwardAngle <= 20.0f)
            {
                myController.forwardAngle = 20.0f;
            }
        }

        if (Vector3.Distance(targetPos, transform.position) <= 0.001f)
        {
            animator.SetInteger("trans", 0);

            GameObject.Find("CollisionDetector").GetComponent<BoxCollider>().enabled = true;

            //�R�E������ǐՃX�e�[�g�ɐ؂�ւ���
            BatController batCon = gameObject.GetComponent<BatController>();
            batCon.ChangeState(GetComponent<batMove>());
            return;
        }
    }

    private Vector3 SearchCeiling(Vector3 _rayPos)
    {
        Vector3 _rayPosition = _rayPos;
        Vector3 _targetPos;

        Ray ray = new Ray(_rayPosition, Vector3.up);

        int layerMask = 1 << 9;

        //���C���[�}�X�N��"cave"�i���A�j�ɂ��ă��C������s��
        if (Physics.Raycast(ray, out hit, 1000.0f, layerMask))
        {
            //���C���V��ɏՓ˂��Ă���ꍇ�̓^�[�Q�b�g���W�ɐݒ肷��B
            Vector3 _targetVec = Vector3.up * hit.distance;
            _targetPos = _rayPosition + _targetVec;
        }
        else
        {
            _targetPos = Vector3.zero;

            //���A�O�ɂ��邽�߃R�E���������ł�����
            myController.Damage(1);
        }

        return _targetPos;
    }

    public void OnDetectorObject(Collider collider)
    {
        if (nowAction == e_Action.sticking)
        {
            GameObject.Find("CollisionDetector").GetComponent<BoxCollider>().enabled = false;
        }
    }

    public float NeedHight(float _amountMoved, float _defaltHight, float _maxHight, float _maxDistance)
    {
        float a, q, x, y;
        q = _defaltHight;
        y = _maxHight;
        x = _maxDistance;

        a = (-1.0f * (q - y)) / (x * x);

        float _hight = a * (_amountMoved * _amountMoved) + q;
        return _hight;
    }
}
