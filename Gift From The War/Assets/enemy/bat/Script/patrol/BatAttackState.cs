using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttackState : State<BatPatrolState>
{
    public BatAttackState(BatPatrolState owner) : base(owner) { }
    bool switchAnime;

    public override void Enter()
    {
        owner.agent.isStopped = true;

        owner.agent.destination = owner.bat.transform.position;

        switchAnime = true;
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false) switchAnime = false;

        owner.animator.SetTrigger("Attack");
    }

    public override void Execute()
    {
        //�A�j���[�V�������؂�ւ���Ă��Ȃ��ꍇ
        if (switchAnime == false)
        {
            CheckSwitchAnime();
            return;
        }

        //�A�j���[�V�������I�����Ă���ꍇ
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false)
        {
            owner.ChangeState(e_BatPatrolState.Tracking);
            return;
        }
    }

    public override void Exit()
    {
        owner.agent.isStopped = false;
    }

    void CheckSwitchAnime()
    {
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == true)
        {
            switchAnime = true;
        }
    }
}
