using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatShakeHeadState : State<BatPatrolState>
{
    public BatShakeHeadState(BatPatrolState owner) : base(owner) { }
    bool switchAnime;
    bool movement;

    public override void Enter()
    {
        movement = true;
    }

    public override void Execute()
    {
        //移動している場合
        if (movement == true)
        {
            //プレイヤーの消息が切れた座標に付いた場合
            if (owner.agent.remainingDistance <= owner.agent.stoppingDistance)
            {
                //アニメーションを切り替える
                switchAnime = true;
                if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("ShakeHead") == false) switchAnime = false;
                owner.animator.SetTrigger("ShakeHead");
                movement = false;
            }
            return;
        }

        //アニメーションが切り替わっていない場合
        if (switchAnime == false)
        {
            CheckSwitchAnime();
            return;
        }

        //アニメーションが終了している場合
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("ShakeHead") == false)
        {
            owner.ChangeState(e_BatPatrolState.MoveWayPoints);
            return;
        }
    }

    public override void Exit()
    {

    }

    void CheckSwitchAnime()
    {
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("ShakeHead") == true)
        {
            switchAnime = true;
        }
    }
}
