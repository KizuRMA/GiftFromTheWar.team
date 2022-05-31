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

        float distance = Vector3.Distance(owner.bat.transform.position,owner.player.transform.position);
        if (distance <= 1.0f)
        {
            owner.ChangeState(e_BatPatrolState.Attack);
            return;
        }

        var target = owner.player.GetComponent<playerAbnormalcondition>();
        if (target.IsHowling() == false)
        {
            owner.ChangeState(e_BatPatrolState.ShakeHead);
            return;
        }
    }

    public override void Exit()
    {

    }
}
