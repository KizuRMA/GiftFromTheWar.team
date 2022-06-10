using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAttackState : State<DogState>
{
    public DogAttackState(DogState owner) : base(owner) { }
    private Rigidbody rig;
    float time;
    bool switchAnime;

    public override void Enter()
    {
        time = 0;

        //アニメーションを変化
        owner.animator.SetInteger("trans", 0);
        owner.animator.SetTrigger("Attack");
        switchAnime = true;
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false) switchAnime = false;

        rig = owner.dog.GetComponent<Rigidbody>();
        owner.agent.isStopped = true;
        owner.agent.updatePosition = false;
        owner.agent.updateUpAxis = false;

        rig.isKinematic = false;
        owner.agent.destination = owner.dog.transform.position;
    }

    public override void Execute()
    {
        //アニメーションが切り替わっていない場合
        if (switchAnime == false)
        {
            CheckSwitchAnime();
            return;
        }

        //Debug.Log(owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            owner.animator.speed = 0;
        }
        //time += Time.deltaTime;
        //if (time >= 1.0f)
        //{
        //    owner.ChangeState(e_DogState.Tracking);
        //}
    }

    public override void Exit()
    {
        owner.agent.isStopped = false;
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
