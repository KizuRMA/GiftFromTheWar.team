using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogSearchState : State<DogState>
{
    public DogSearchState(DogState owner) : base(owner) { }

    NavMeshAgent agent;
    Vector3[] targetPos;
    const int arrayMax = 60;
    bool canSetFlg;

    public override void Enter()
    {
        owner.animator.SetInteger("trans", 0);
        canSetFlg = true;
        targetPos = new Vector3[arrayMax];
        for (int i = 0; i < arrayMax; i++)
        {
            targetPos[i] = owner.player.transform.position;
        }

        agent = owner.agent;
        agent.speed = owner.SearchSpeed;
    }

    public override void Execute()
    {
        float dis = Vector3.Distance(owner.player.transform.position, owner.dog.transform.position);

        if (canSetFlg == true && dis <= 75.0f)
        {
            owner.StartCoroutine(TargetCoroutine());
        }

       

        if (dis <= 1.0f)
        {
            owner.ChangeState(e_DogState.Tracking);
            return;
        }
       
        if (owner.canVigilance == true && dis <= 5.0f)
        {
            owner.ChangeState(e_DogState.Vigilance);
            return;
        }

      
    }

    public override void Exit()
    {

    }

    private IEnumerator TargetCoroutine()
    {
        canSetFlg = false;
        yield return new WaitForSeconds(0.1f);

        for (int i = arrayMax - 1; i > 0; i--)
        {
            targetPos[i] = targetPos[i - 1];
        }

        targetPos[0] = owner.player.transform.position;
        agent.destination = targetPos[arrayMax - 1];
        canSetFlg = true;
    }


}
