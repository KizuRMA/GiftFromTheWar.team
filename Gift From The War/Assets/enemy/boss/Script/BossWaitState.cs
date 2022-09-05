using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BossWaitState : State<BossState>
{
    public BossWaitState(BossState owner) : base(owner) { }
    private NavMeshAgent agent;

    public override void Enter()
    {
        agent = owner.agent;
    }

    public override void Execute()
    {
        agent.destination = owner.player.transform.position;
        if (Input.GetKeyDown(KeyCode.Return) == true)
        {
            owner.ChangeState(e_BossState.Crash);
        }
    }

    public override void Exit()
    {
        owner.WarpPosition(owner.transform.position);
    }
}
