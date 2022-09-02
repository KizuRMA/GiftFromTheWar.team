using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrackingState : State<BossState>
{
    public BossTrackingState(BossState owner) : base(owner) { }

    public override void Enter()
    {
        //追いかけるターゲットを設定
        owner.agent.destination = owner.wayPoint.wayPoints[owner.currentWaypointIndex].position;
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
