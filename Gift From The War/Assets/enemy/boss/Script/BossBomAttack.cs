using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBomAttack : State<BossState>
{
    public BossBomAttack(BossState owner) : base(owner) { }

    public override void Enter()
    {
        owner.animator.SetTrigger("Attack");
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
