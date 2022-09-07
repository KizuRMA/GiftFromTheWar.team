using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrackingState : State<BossState>
{
    public BossTrackingState(BossState owner) : base(owner) { }

    public override void Enter()
    {
        if (owner.attackFlg == true)
        {
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
