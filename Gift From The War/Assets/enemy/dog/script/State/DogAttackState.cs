using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAttackState : State<DogState>
{
    public DogAttackState(DogState owner) : base(owner) { }
    float time;

    public override void Enter()
    {
        time = 0;

        //警戒アニメーションに変更する
        owner.animator.SetInteger("trans", 0);
        owner.animator.SetTrigger("Attack");
        owner.agent.isStopped = true;
        owner.agent.destination = owner.dog.transform.position;
    }

    public override void Execute()
    {
        time += Time.deltaTime;
        if (time >= 2.0f)
        {
            owner.ChangeState(e_DogState.Tracking);
        }
    }

    public override void Exit()
    {
        owner.agent.isStopped = false;
    }
}
