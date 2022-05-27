using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogVigilanceState : State<DogState>
{
    public DogVigilanceState(DogState owner) : base(owner) { }
    bool switchAnime;

    public override void Enter()
    {
        owner.animator.SetTrigger("Vigilance");

        switchAnime = true;
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("metarig_action_Vigilance") == false) switchAnime = false;
       
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

        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("metarig_action_Vigilance") == false)
        {
            owner.ChangeState(e_DogState.Search);
        }
    }

    public override void Exit()
    {
        owner.endAnimationFlg = false;
        owner.agent.isStopped = false;
        owner.StartCoroutine(CoolDownCoroutine());
    }

    private IEnumerator CoolDownCoroutine()
    {
        owner.canVigilance = false;
        yield return new WaitForSeconds(3.0f);
        owner.canVigilance = true;
    }

    void CheckSwitchAnime()
    {
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("metarig_action_Vigilance") == true)
        {
            switchAnime = true;
        }
    }
}
