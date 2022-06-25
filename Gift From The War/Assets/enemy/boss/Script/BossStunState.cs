using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStunState : State<BossState>
{
    public BossStunState(BossState owner) : base(owner) { }

    public override void Enter()
    {
        owner.animator.SetTrigger("Stun");

    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
