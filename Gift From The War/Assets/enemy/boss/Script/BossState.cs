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
    [SerializeField] public Animator animator;
    [SerializeField] public WayPoint wayPoint;
    [SerializeField] public float trackingSpeed = 1.0f;

    private int currentWaypointIndex;

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
        CalcVelocityExample example = player.GetComponent<CalcVelocityExample>();
        float _speed = example.nowSpeed;

        if (_speed >= 100.0f)
        {
            return true;
        }
        return false;
    }
}
