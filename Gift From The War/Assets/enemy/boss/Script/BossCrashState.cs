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
        owner.agent.isStopped = true;
        owner.agent.updatePosition = false;
    }

    public override void Execute()
    {
        time += Time.deltaTime;
        //f (time >= 1.0f)
        {
            owner.ChangeState(e_BossState.Tracking);
        }
    }

    public override void Exit()
    {
        owner.agent.isStopped = false;
        owner.agent.updatePosition = true;
    }
}
