using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatDeadState : State<BatPatrolState>
{
    public BatDeadState(BatPatrolState owner) : base(owner) { }

    public override void Enter()
    {
        //ナビメッシュを切る
        owner.agent.isStopped = true;
        owner.agent.updatePosition = false;
        owner.agent.updateUpAxis = false;
    }

    public override void Execute()
    {
        AudioManager.Instance.PlaySE("BatDead",owner.gameObject,isLoop:false);
        owner.DestroyBat();
    }

    public override void Exit()
    {

    }
}
