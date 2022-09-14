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

    public bool IsRouteChenge
    {
        get
        {
            return wayPoint != owner.wayPoint;
        }
    }

    public override void Enter()
    {
        owner.animator.SetInteger("trans", 0);
        owner.animator.SetFloat("AnimationSpeed", 1.0f);
        wayPoint = owner.wayPoint;
        agent = owner.agent;
        owner.agent.speed = owner.moveWayPointSpeed;

        //�ł��߂�WayPoint�̓Y����������
        currentWaypointIndex = NearestWayPointIndex();
        owner.agent.destination = wayPoint.wayPoints[currentWaypointIndex].position;

        owner.ChangeUltrasound(e_UltrasoundState.Large);
    }

    public override void Execute()
    {
        if (IsRouteChenge == true)
        {
            RouteChange();
        }

        Vector3 _nowPos = new Vector3(owner.bat.transform.position.x, agent.destination.y, owner.bat.transform.position.z);
        float targetDis = Vector3.Distance(_nowPos, agent.destination);

        //�ړI�n�܂ŋ������߂��ꍇ
        if (targetDis <= 0.1f)
        {
            // �ړI�n�̔ԍ����P�X�V�i�E�ӂ���]���Z�q�ɂ��邱�ƂŖړI�n�����[�v�������j
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoint.wayPoints.Count;
            // �ړI�n�����̏ꏊ�ɐݒ�
            agent.destination = wayPoint.wayPoints[currentWaypointIndex].position;
        }

        float distance = Vector3.Distance(owner.player.transform.position, owner.bat.transform.position);

        var target = owner.player.GetComponent<playerAbnormalcondition>();
        if (distance <= 1.0f)
        {
            target.AddHowlingAbnormal();
            owner.ChangeState(e_BatPatrolState.Tracking);
            return;
        }

        //�����g�𔭎˂��Ă��邩�H
        if (owner.currentUltrasound == null) return;

        if (owner.currentUltrasound.CheckHit() == true)
        {
            target.AddHowlingAbnormal();
            owner.ChangeState(e_BatPatrolState.Tracking);
            return;
        }

        if (distance <= owner.reactionDis && target.IsHowling() == true)
        {
            owner.ChangeState(e_BatPatrolState.Tracking);
            return;
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

    private void RouteChange()
    {
        wayPoint = owner.wayPoint;
        currentWaypointIndex = NearestWayPointIndex();
        owner.agent.destination = wayPoint.wayPoints[currentWaypointIndex].position;
    }

    private int NearestWayPointIndex()
    {
        Vector3 _batPos = owner.bat.transform.position;
        float _minDistance = float.MaxValue;
        int _index = 0;

        NavMeshPath navMeshPath = new NavMeshPath();

        //���ݒn����ł��߂�WayPoint���^�[�Q�b�g���W�ɂ���
        foreach (var _wayPoint in wayPoint.wayPoints)
        {
            agent.CalculatePath(_wayPoint.position, navMeshPath);
            float dis = 0.0f;

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
                _index = wayPoint.wayPoints.IndexOf(_wayPoint);
            }
        }

        return _index;
    }
}
