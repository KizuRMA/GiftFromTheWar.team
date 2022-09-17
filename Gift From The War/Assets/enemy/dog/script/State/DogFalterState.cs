using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogFalterState : State<DogState>
{
    public DogFalterState(DogState owner) : base(owner) { }
    bool switchAnime;

    public override void Enter()
    {
        //怯みアニメーションに変更する
        owner.animator.SetTrigger("Falter");
        switchAnime = true;
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Falter") == false) switchAnime = false;

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
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Falter") == false)
        {
            owner.ChangeState(e_DogState.Tracking);
            return;
        }
    }

    public override void Exit()
    {
        owner.agent.isStopped = true;
        owner.agent.Warp(owner.dog.transform.position);
        if (NavMesh.SamplePosition(owner.dog.transform.position, out NavMeshHit navMeshHit, 10, NavMesh.AllAreas))
        {
            owner.agent.Warp(navMeshHit.position);
        }
    }

    void CheckSwitchAnime()
    {
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Falter") == true)
        {
            switchAnime = true;
        }
    }
}
