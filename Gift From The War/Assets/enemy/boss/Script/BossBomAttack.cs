using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBomAttack : State<BossState>
{
    public BossBomAttack(BossState owner) : base(owner) { }
    bool switchAnime;

    public override void Enter()
    {
        owner.animator.SetTrigger("Attack");

        switchAnime = true;
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false) switchAnime = false;
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
            owner.ChangeState(e_BossState.Tracking);
            return;
        }
    }

    public override void Exit()
    {

    }

    void CheckSwitchAnime()
    {
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == true)
        {
            switchAnime = true;
        }
    }
}
