using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatDeadState : State<BatPatrolState>
{
    public BatDeadState(BatPatrolState owner) : base(owner) { }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        owner.DestroyBat();
    }

    public override void Exit()
    {

    }
}
