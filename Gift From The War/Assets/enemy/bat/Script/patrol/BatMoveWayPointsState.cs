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
        // 目的地点までの距離(remainingDistance)が目的地の手前までの距離(stoppingDistance)以下になったら
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // 目的地の番号を１更新（右辺を剰余演算子にすることで目的地をループさせれる）
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoint.wayPoints.Count;
            // 目的地を次の場所に設定
            agent.destination = wayPoint.wayPoints[currentWaypointIndex].position;
        }
    }

    public override void Exit()
    {

    }
}
