using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum e_BossState
{
    Tracking,
    BomAttack,
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

    private int currentWaypointIndex;
    private GameObject generatedGrenade;

    void Start()
    {
        stateList.Add(new BossTrackingState(this));
        stateList.Add(new BossBomAttack(this));

        ChangeState(e_BossState.Tracking);

        stateMachine = new StateMachine<BossState>();

        //=============
        //�ϐ��̏�����
        //=============
        currentWaypointIndex = 0;
        agent.speed = trackingSpeed;

        //�ǂ�������^�[�Q�b�g��ݒ�
        agent.destination = wayPoint.wayPoints[currentWaypointIndex].position;
    }

    protected override void Update()
    {
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
        if (IsCurrentState(e_BossState.BomAttack) == true) return false;

        CalcVelocityExample example = player.GetComponent<CalcVelocityExample>();
        float _speed = example.nowSpeed;

        if (_speed >= 3.0f)
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
    }

    public void Catch()
    {
        generatedGrenade.transform.parent = CatchPosition.transform.parent;
        Rigidbody rd = generatedGrenade.GetComponent<Rigidbody>();
        rd.useGravity = false;
        rd.isKinematic = true;
        generatedGrenade.transform.localPosition = CatchPosition.transform.localPosition;
    }

    public void Throw()
    {
        generatedGrenade.transform.parent = null;

        Rigidbody rd = generatedGrenade.GetComponent<Rigidbody>();
        rd.useGravity = true;
        rd.isKinematic = false;

        CapsuleCollider collider = generatedGrenade.GetComponent<CapsuleCollider>();
        collider.isTrigger = false;

        // �W�I�̍��W
        Vector3 targetPosition = player.transform.position;

        // �ˏo�p�x
        float angle = 1.0f;

        // �ˏo���x���Z�o
        Vector3 velocity = CalculateVelocity(generatedGrenade.transform.position, targetPosition, angle);

        // �ˏo
        rd.AddForce(velocity * rd.mass, ForceMode.Impulse);

        var random = new System.Random();
        var min = -3;
        var max = 3;

        var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
        rd.AddTorque(vect, ForceMode.Impulse);
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
}
