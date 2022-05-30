using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatMoveWayPointsState : State<BatPatrolState>
{
    public BatMoveWayPointsState(BatPatrolState owner) : base(owner) { }
    private WayPoint wayPoint;
    private NavMeshAgent agent;
    private BaseUltrasound ultrasound;
    private int currentWaypointIndex = 0;

    public override void Enter()
    {
        owner.animator.SetInteger("trans", 0);
        wayPoint = owner.wayPoint;
        agent = owner.agent;
        owner.agent.destination = wayPoint.wayPoints[currentWaypointIndex].position;
    }

    public override void Execute()
    {
        // �ړI�n�_�܂ł̋���(remainingDistance)���ړI�n�̎�O�܂ł̋���(stoppingDistance)�ȉ��ɂȂ�����
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // �ړI�n�̔ԍ����P�X�V�i�E�ӂ���]���Z�q�ɂ��邱�ƂŖړI�n�����[�v�������j
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoint.wayPoints.Count;
            // �ړI�n�����̏ꏊ�ɐݒ�
            agent.destination = wayPoint.wayPoints[currentWaypointIndex].position;
        }
    }

    public override void Exit()
    {

    }
}
