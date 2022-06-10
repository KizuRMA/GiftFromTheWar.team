using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBlowedAwayState : State<DogState>
{
    public DogBlowedAwayState(DogState owner) : base(owner) { }
    private Rigidbody rd;

    public override void Enter()
    {
        //ナビメッシュを切る
        owner.agent.isStopped = true;
        owner.agent.updatePosition = false;
        owner.agent.updateUpAxis = false;

        rd = owner.GetComponent<Rigidbody>();
        rd.isKinematic = false;

        Vector3 _upVec = owner.transform.up.normalized * 3;
        Vector3 _forwardVec = owner.transform.forward .normalized * -3;

        rd.AddForce(_upVec, ForceMode.Impulse);
        rd.AddForce(_forwardVec, ForceMode.Impulse);
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
