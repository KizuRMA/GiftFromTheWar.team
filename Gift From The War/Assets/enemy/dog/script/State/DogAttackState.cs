using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogAttackState : State<DogState>
{
    public DogAttackState(DogState owner) : base(owner) { }
    private Rigidbody rig;
    bool switchAnime;

    public override void Enter()
    {

        //アニメーションを変化
        owner.animator.SetInteger("trans", 0);
        owner.animator.SetFloat("Speed", 1.0f);
        owner.animator.SetTrigger("Attack");
        switchAnime = true;
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false) switchAnime = false;

        rig = owner.dog.GetComponent<Rigidbody>();
        owner.agent.isStopped = true;
        owner.agent.updatePosition = false;
        owner.agent.updateUpAxis = false;

        rig.isKinematic = false;
    }

    public override void Execute()
    {
        //アニメーションが切り替わっていない場合
        if (switchAnime == false)
        {
            CheckSwitchAnime();
            return;
        }

        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            owner.ChangeState(e_DogState.Tracking);
            return;
        }
    }

    public override void Exit()
    {
        owner.agent.isStopped = false;
        owner.agent.updatePosition = true;
        owner.agent.updateUpAxis = true;

        owner.agent.Warp(owner.dog.transform.position);

        rig.constraints = RigidbodyConstraints.None;
        rig.isKinematic = true;
    }

    void CheckSwitchAnime()
    {
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == true)
        {
            switchAnime = true;
        }
    }
}
