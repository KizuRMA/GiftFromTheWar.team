using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogTrakingState : State<DogState>
{
    public DogTrakingState(DogState owner) : base(owner) { }

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

        if (owner.agent.remainingDistance <= 1.5f)
        {
            owner.ChangeState(e_DogState.Attack);
        }
    }

    public override void Exit()
    {

    }
}
