using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTrackingState : State<BatPatrolState>
{
    public BatTrackingState(BatPatrolState owner) : base(owner) { }

    public override void Enter()
    {
        owner.agent.speed = owner.trackingSpeed;
        owner.animator.SetFloat("AnimationSpeed",1.3f);
    }

    public override void Execute()
    {
        owner.agent.destination = owner.player.transform.position;
        var target = owner.player.GetComponent<playerAbnormalcondition>();
        float distance = Vector3.Distance(owner.bat.transform.position,owner.player.transform.position);

        if (distance <= 0.5f)
        {
            target.AddHowlingAbnormal();
            owner.ChangeState(e_BatPatrolState.Attack);
            return;
        }

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
