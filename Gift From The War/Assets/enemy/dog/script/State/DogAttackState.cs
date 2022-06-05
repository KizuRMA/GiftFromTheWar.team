using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAttackState : State<DogState>
{
    public DogAttackState(DogState owner) : base(owner) { }
    private Rigidbody rigidbody;
    float time;

    public override void Enter()
    {
        time = 0;

        //警戒アニメーションに変更する
        owner.animator.SetInteger("trans", 0);
        owner.animator.SetTrigger("Attack");
        rigidbody = owner.dog.GetComponent<Rigidbody>();
        owner.agent.isStopped = true;
        rigidbody.isKinematic = false;
        owner.agent.destination = owner.dog.transform.position;
    }

    public override void Execute()
    {
        time += Time.deltaTime;
        if (time >= 1.0f)
        {
            owner.ChangeState(e_DogState.Tracking);
        }
    }

    public override void Exit()
    {
        owner.agent.isStopped = false;
        rigidbody.isKinematic = true;
    }
}
