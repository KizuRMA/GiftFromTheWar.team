using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTrackingState : State<BatPatrolState>
{
    public BatTrackingState(BatPatrolState owner) : base(owner) { }

    public override void Enter()
    {
        owner.agent.speed = owner.trackingSpeed;
        owner.animator.SetFloat("AnimationSpeed",owner.trackingAnimSpeed);
        owner.ChangeUltrasound(e_UltrasoundState.Beam);
    }

    public override void Execute()
    {
        owner.agent.destination = owner.player.transform.position;
        var target = owner.player.GetComponent<playerAbnormalcondition>();

        Vector3 _batPos = owner.bat.transform.position + (owner.bat.transform.forward * 0.5f);
        float distance = Vector3.Distance(_batPos,owner.player.transform.position);

        if (distance <= 1.0f)
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
