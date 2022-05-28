using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAttackState : State<DogState>
{
    public DogAttackState(DogState owner) : base(owner) { }

    public override void Enter()
    {
        //警戒アニメーションに変更する
        owner.animator.SetTrigger("Attack");
        owner.agent.isStopped = true;
        owner.agent.destination = owner.dog.transform.position;
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        owner.agent.isStopped = false;
    }
}
