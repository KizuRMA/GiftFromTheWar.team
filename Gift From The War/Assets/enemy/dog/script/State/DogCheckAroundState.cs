using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogCheckAroundState : State<DogState>
{
    public DogCheckAroundState(DogState owner) : base(owner) { }
    bool switchAnime;

    public override void Enter()
    {
        //�x���A�j���[�V�����ɕύX����
        owner.animator.SetTrigger("CheckAround");
        switchAnime = true;
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("CheckAround") == false) switchAnime = false;

        owner.agent.isStopped = true;
        owner.agent.destination = owner.dog.transform.position;
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
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("CheckAround") == false)
        {
            owner.ChangeState(e_DogState.Search);
            return;
        }
    }

    public override void Exit()
    {
        owner.agent.isStopped = false;
    }

    void CheckSwitchAnime()
    {
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("CheckAround") == true)
        {
            switchAnime = true;
        }
    }


}
