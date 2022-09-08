using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossTrackingState : State<BossState>
{
    public BossTrackingState(BossState owner) : base(owner) { }

    public override void Enter()
    {
        if (owner.attackFlg == true)
        {
            NavMeshPath navMeshPath = new NavMeshPath();
            owner.agent.CalculatePath(owner.player.transform.position, navMeshPath);

            float dis = 0.0f;

            Vector3 corner = owner.transform.position; ;
            for (int i = 0; i < navMeshPath.corners.Length; i++)
            {
                Vector3 corner2 = navMeshPath.corners[i];
                dis += Vector3.Distance(corner, corner2);
                corner = corner2;
            }

            if (dis >= 25.0f)
            {
                owner.agent.speed = owner.trackingSpeed + 1.5f;
            }
            else
            {
                owner.agent.speed = owner.trackingSpeed;
            }

            //追いかけるターゲットを設定
            owner.agent.destination = owner.wayPoint.wayPoints[owner.currentWaypointIndex].position;
        }
    }

    public override void Execute()
    {
        if (owner.attackFlg == false)
        {
            //追いかけるターゲットを設定
            owner.agent.destination = owner.player.transform.position;
        }


    }

    public override void Exit()
    {

    }
}
