using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum e_BossState
{
    Tracking,
    BomAttack,
    Stun,
    Wait,
    Crash,
    Sleep,
}

public class BossState : StatefulObjectBase<BossState, e_BossState>
{
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject myGameObject;
    [SerializeField] public GameObject grenadePrefab;
    [SerializeField] public Transform createPosition;
    [SerializeField] public Transform CatchPosition;
    [SerializeField] public Animator animator;
    [SerializeField] public WayPoint wayPoint;
    [SerializeField] public float trackingSpeed = 1.0f;
    [SerializeField] public float attackIntervalSecond = 1;
    [SerializeField] public float attackRate = 70;
    [SerializeField] public float life;

    [System.NonSerialized] public int currentWaypointIndex;
    [System.NonSerialized] public GameObject generatedGrenade;
    [System.NonSerialized] public bool attackFlg;
    [System.NonSerialized] public bool getupFlg;

    private Vector3 throwTargetPos;
    private float attackTimeCounter = 0;
    private Vector3 destination;

    void Start()
    {
        stateMachine = new StateMachine<BossState>();

        stateList.Add(new BossTrackingState(this));
        stateList.Add(new BossBomAttack(this));
        stateList.Add(new BossStunState(this));
        stateList.Add(new BossWaitState(this));
        stateList.Add(new BossCrashState(this));
        stateList.Add(new BossSleepState(this));

        ChangeState(e_BossState.Sleep);

        //=============
        //�ϐ��̏�����
        //=============
        attackFlg = false;
        getupFlg = false;
        currentWaypointIndex = 0;
        agent.speed = trackingSpeed;
    }

    protected override void Update()
    {
        destination = agent.destination;
        base.Update();

        //�ړI�n�̍X�V�͏펞��������
        DestinationUpdate();

        //�U�����邩�m�F
        if (IsAttack() == true)
        {
            ChangeState(e_BossState.BomAttack);
        }
    }

    private void DestinationUpdate()    //�ړI�l���X�V���鏈��
    {
        if (IsCurrentState(e_BossState.Stun) == true ||
            IsCurrentState(e_BossState.Wait) == true ||
            IsCurrentState(e_BossState.Crash) == true||
            IsCurrentState(e_BossState.Sleep) == true) return;
        if (attackFlg == false) return;

        Vector3 _nowPos = new Vector3(transform.position.x, agent.destination.y, transform.position.z);
        float targetDis = Vector3.Distance(_nowPos, agent.destination);

        // �ړI�n�_�܂ł̋���(remainingDistance)���ړI�n�̎�O�܂ł̋���(stoppingDistance)�ȉ��ɂȂ�����
        if (targetDis <= 0.1f)
        {
            // �ړI�n�̔ԍ����P�X�V�i�E�ӂ���]���Z�q�ɂ��邱�ƂŖړI�n�����[�v�������j
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoint.wayPoints.Count;
            // �ړI�n�����̏ꏊ�ɐݒ�
            agent.destination = wayPoint.wayPoints[currentWaypointIndex].position;
        }
    }

    public bool IsAttack()
    {
        //�{�X�U�����@�܂��́@�X�^�����@�܂��́@�ҋ@��Ԃł��鎞�͑������^�[��
        if (IsCurrentState(e_BossState.Tracking) == false || attackFlg == false) return false;

       attackTimeCounter += Time.deltaTime;
       if(attackTimeCounter <= attackIntervalSecond) return false;

        attackTimeCounter = 0;
        int max = 100;

        int random = Random.Range(0,max + 1);

        if (random <= attackRate)
        {
            return true;
        }
        return false;
    }

    public void Create()
    {
        if (grenadePrefab == null) return;

        generatedGrenade = Instantiate(grenadePrefab);
        generatedGrenade.transform.parent = transform;
        generatedGrenade.transform.position = createPosition.position;

        var random = new System.Random();
        var min = -3;
        var max = 3;

        Vector3 _targetVec = player.transform.position - transform.position;
        throwTargetPos = player.transform.position;
        _targetVec = _targetVec.normalized * random.Next(1, max + 1);

        Vector3 _randomPos = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));

        throwTargetPos += _targetVec;
        throwTargetPos += _randomPos;
    }

    public void Catch()
    {
        if (generatedGrenade == null) return;

        generatedGrenade.transform.parent = CatchPosition.transform.parent;
        Rigidbody rd = generatedGrenade.GetComponent<Rigidbody>();
        rd.useGravity = false;
        rd.isKinematic = true;
        generatedGrenade.transform.localPosition = CatchPosition.transform.localPosition;
    }

    public void Throw()
    {
        if (generatedGrenade == null) return;

        generatedGrenade.transform.parent = null;

        Rigidbody rd = generatedGrenade.GetComponent<Rigidbody>();
        rd.useGravity = true;
        rd.isKinematic = false;

        CapsuleCollider collider = generatedGrenade.GetComponent<CapsuleCollider>();
        collider.isTrigger = false;

        CalcVelocityExample example = player.GetComponent<CalcVelocityExample>();
        float _speed = example.nowSpeed;

        if (_speed >= 3.0f)
        {
            // �W�I�̍��W
            throwTargetPos = player.transform.position;
        }


        // �ˏo�p�x
        float angle = 1.0f;

        // �ˏo���x���Z�o
        Vector3 velocity = CalculateVelocity(generatedGrenade.transform.position, throwTargetPos, angle);

        // �ˏo
        rd.AddForce(velocity * rd.mass, ForceMode.Impulse);

        var random = new System.Random();
        var min = -3;
        var max = 3;

        var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
        rd.AddTorque(vect, ForceMode.Impulse);
        generatedGrenade = null;
    }

    public void GrenadeRelease()
    {
        if (generatedGrenade == null) return;

        generatedGrenade.transform.parent = null;
        Rigidbody rd = generatedGrenade.GetComponent<Rigidbody>();
        rd.useGravity = true;
        rd.isKinematic = false;
    }

    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        // �ˏo�p�����W�A���ɕϊ�
        float rad = angle * Mathf.PI / 180;

        // ���������̋���x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // ���������̋���y
        float y = pointA.y - pointB.y;

        // �Ε����˂̌����������x�ɂ��ĉ���
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // �����𖞂����������Z�o�ł��Ȃ����Vector3.zero��Ԃ�
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }

    public void ExpDamage(int _damage,Vector3 _hypocenter)
    {
        ChangeState(e_BossState.Stun);

        if (life < 0) return;

        life -= _damage;

        if (life > 0) return;
    }

    public void WarpPosition(Vector3 _pos)
    {
        //Warp()�֐����d�l�����destination���ꏏ�ɕύX���Ă��܂�
        agent.Warp(_pos);
        agent.destination = destination;
        transform.position = _pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var target = other.transform.GetComponent<playerAbnormalcondition>();
            if (null == target) return;

            target.Damage(10.0f);
        }
    }
}
