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
        owner.animator.SetFloat("Speed", 1.1f);
        owner.agent.speed = owner.TrakingSpeed;
        owner.agent.stoppingDistance = 1.5f;
    }

    public override void Execute()
    {
        owner.agent.destination = owner.player.transform.position;

        float targetDis = Vector3.Distance(owner.dog.transform.position,owner.player.transform.position);

        if (targetDis <= 2.0f)
        {
            owner.ChangeState(e_DogState.Attack);
            return;
        }

        if (targetDis >= 30.0f)
        {
            owner.ChangeState(e_DogState.CheckAround);
            return;
        }
    }

    public override void Exit()
    {
        owner.animator.SetFloat("Speed", 1.0f);
    }
}
