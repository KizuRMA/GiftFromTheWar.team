using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogTrackingState : State<DogState>
{
    public DogTrackingState(DogState owner) : base(owner) { }

    // Start is called before the first frame update
    public override void Enter()
    {
        owner.animator.SetInteger("trans", 1);
        owner.agent.speed = owner.TrakingSpeed;
        owner.agent.stoppingDistance = 1.5f;
    }

    public override void Execute()
    {
        owner.agent.destination = owner.player.transform.position;

        float targetDis = owner.agent.remainingDistance;

        if (targetDis <= 1.5f)
        {
            owner.ChangeState(e_DogState.Attack);
            return;
        }

        if (targetDis >= 10.0f)
        {
            owner.ChangeState(e_DogState.CheckAround);
            return;
        }
    }

    public override void Exit()
    {

    }
}
