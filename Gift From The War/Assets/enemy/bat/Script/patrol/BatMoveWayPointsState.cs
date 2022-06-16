using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatMoveWayPointsState : State<BatPatrolState>
{
    public BatMoveWayPointsState(BatPatrolState owner) : base(owner) { }
    private WayPoint wayPoint;
    private NavMeshAgent agent;
    private int currentWaypointIndex;

    public override void Enter()
    {
        owner.animator.SetInteger("trans", 0);
        owner.animator.SetFloat("AnimationSpeed", 1.0f);
        wayPoint = owner.wayPoint;
        agent = owner.agent;
        owner.agent.speed = owner.moveWayPointSpeed;

        Vector3 _batPos = owner.bat.transform.position;
        float _minDistance = float.MaxValue;

        NavMeshPath navMeshPath = new NavMeshPath(); ;

        //���ݒn����ł��߂�WayPoint���^�[�Q�b�g���W�ɂ���
        foreach (var _wayPoint in wayPoint.wayPoints)
        {
            agent.CalculatePath(_wayPoint.position, navMeshPath);
            float dis = agent.remainingDistance;

            Vector3 corner = _batPos;
            for (int i = 0; i < navMeshPath.corners.Length; i++)
            {
                Vector3 corner2 = navMeshPath.corners[i];
                dis += Vector3.Distance(corner, corner2);
                corner = corner2;
            }

            if (dis < _minDistance)
            {
                _minDistance = dis;
                currentWaypointIndex = wayPoint.wayPoints.IndexOf(_wayPoint);
            }
        }

        owner.agent.destination = wayPoint.wayPoints[currentWaypointIndex].position;
        owner.ChangeUltrasound(e_UltrasoundState.Large);
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

        if (owner.currentUltrasound == null) return;

        if (owner.currentUltrasound.CheckHit() == true)
        {
            var target = owner.player.GetComponent<playerAbnormalcondition>();
            target.AddHowlingAbnormal();
            owner.ChangeState(e_BatPatrolState.Tracking);
        }

        if (owner.currentUltrasound.IsAlive == false)
        {
            owner.untilLaunch = 0;
            owner.currentUltrasound.Init();
        }
    }

    public override void Exit()
    {
        owner.untilLaunch = 0;
        owner.currentUltrasound.Start();
    }
}
