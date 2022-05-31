using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTrackingState : State<BatPatrolState>
{
    public BatTrackingState(BatPatrolState owner) : base(owner) { }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        owner.agent.destination = owner.player.transform.position;
    }

    public override void Exit()
    {

    }
}
