using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogTrackingState : State<DogState>
{
    public DogTrackingState(DogState owner) : base(owner) { }
    float time;

    // Start is called before the first frame update
    public override void Enter()
    {
        owner.animator.SetInteger("trans", 1);
        owner.animator.SetFloat("Speed", 1.1f);
        owner.agent.speed = owner.TrakingSpeed;
        time = 0;
    }

    public override void Execute()
    {
        owner.agent.destination = owner.player.transform.position;

        if (owner.agent.remainingDistance < 1.5f)
        {
            owner.agent.updatePosition = false;
            owner.agent.updateUpAxis = false;
        }
        else
        {
            owner.agent.updatePosition = true;
            owner.agent.updateUpAxis = true;
        }

        float targetDis = Vector3.Distance(owner.dog.transform.position,owner.player.transform.position);

        //‹——£‚ª‹ß‚¢ó‘Ô‚ª‘±‚¢‚Ä‚¢‚éê‡‚Í•b”‚ðƒJƒEƒ“ƒg‚·‚é
        if (targetDis <= 2.0f)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
        }

        if (IsPossibleToAttack() == true || time >= 2.0f)
        {
            owner.ChangeState(e_DogState.Attack);
            return;
        }

        if (targetDis >= 30.0f)
        {
            owner.ChangeState(e_DogState.CheckAround);
            return;
        }
    }

    public override void Exit()
    {
        owner.animator.SetFloat("Speed", 1.0f);
    }

    private bool IsPossibleToAttack()
    {
        GameObject _dog = owner.dog;
        Vector3 _targetVec = owner.player.transform.position - (_dog.transform.position + new Vector3(0, 0.5f, 0));
        float _dis = _targetVec.magnitude;

        if (_dis >= 3.0f) return false;

        Vector3 _fowardVec = _dog.transform.forward;
        _fowardVec.y = 0;
        _targetVec.y = 0;

        float _dot = Vector3.Dot(_targetVec.normalized, _fowardVec.normalized);

        if (Mathf.Acos(_dot) * Mathf.Rad2Deg >= 10.0f) return false;

        return true;
    }
}
