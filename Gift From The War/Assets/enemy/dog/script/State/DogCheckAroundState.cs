using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogCheckAroundState : State<DogState>
{
    public DogCheckAroundState(DogState owner) : base(owner) { }
    bool switchAnime;

    public override void Enter()
    {
        //警戒アニメーションに変更する
        owner.animator.SetTrigger("CheckAround");
        switchAnime = true;
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("CheckAround") == false) switchAnime = false;

        owner.agent.isStopped = true;
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

        //アニメーションが終了している場合
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
