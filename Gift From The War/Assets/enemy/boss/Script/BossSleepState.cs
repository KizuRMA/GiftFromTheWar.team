using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSleepState : State<BossState>
{
    public BossSleepState(BossState owner) : base(owner) { }

    public override void Enter()
    {
        owner.agent.isStopped = true;
        owner.agent.updatePosition = false;
        owner.agent.updateRotation = false;
    }

    public override void Execute()
    {
        if (owner.getupFlg == true)
        {
            owner.animator.SetTrigger("GetUp");
            owner.animator.SetFloat("Speed", 0.6f);
        }

        //起きるアニメーションに切り替わったとき
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") &&
            owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            owner.ChangeState(e_BossState.Wait);
        }
    }

    public override void Exit()
    {
        owner.agent.isStopped = false;
        owner.agent.updatePosition = true;
        owner.agent.updateRotation = true;
    }
}
