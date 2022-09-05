using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCrashState : State<BossState>
{
    public BossCrashState(BossState owner) : base(owner) { }
    public float time;
    public override void Enter()
    {
        time = 0;
    }

    public override void Execute()
    {
        time += Time.deltaTime;
        if (time <= 1.0f)
        {
            owner.ChangeState(e_BossState.Tracking);
        }
    }

    public override void Exit()
    {
        owner.WarpPosition(owner.transform.position);
    }
}
