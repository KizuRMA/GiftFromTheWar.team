using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogTrackingState : State<DogState>
{
    public DogTrackingState(DogState owner) : base(owner) { }
    public NavMeshAgent agent;
    float time;
    bool rotateOnly;

    public override void Enter()
    {
        agent = owner.agent;
        owner.animator.SetInteger("trans", 1);
        owner.animator.SetFloat("Speed", 1.1f);
        //owner.agent.speed = owner.TrakingSpeed;
        time = 0;

        rotateOnly = false;
        agent.updatePosition = false;
        agent.updateUpAxis = false;
        agent.updateRotation = false;

    }

    public override void Execute()
    {
        owner.animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
        agent.destination = owner.player.transform.position;

        NavMeshPath navMeshPath = new NavMeshPath();

        //現在地から最も近いWayPointをターゲット座標にする
        agent.CalculatePath(owner.player.transform.position, navMeshPath);

        Vector3 _dogPos = owner.dog.transform.position;

        if (navMeshPath.corners.Length > 0)
        {
            Vector3 corner = navMeshPath.corners[0];
            Vector3 _targetVec = corner - new Vector3(_dogPos.x, corner.y, _dogPos.z);

            float _angle = Vector3.SignedAngle(owner.dog.transform.forward, _targetVec, Vector3.up);

            Vector3 _localAngle;
            _localAngle = owner.transform.localEulerAngles;
            _localAngle.y += _angle;
            owner.transform.localEulerAngles = _localAngle;
        }




        /*
        float targetDis = Vector3.Distance(owner.dog.transform.position, owner.player.transform.position);

        if (targetDis < 1.5f)
        {
            if (rotateOnly == false)
            {
                owner.agent.updatePosition = false;
                owner.agent.updateUpAxis = false;
                rotateOnly = true;
            }
        }
        else
        {
            if (rotateOnly == true && targetDis >= 1.5f)
            {
                owner.agent.Warp(owner.dog.transform.position);
                rotateOnly = false;

                owner.agent.updatePosition = true;
                owner.agent.updateUpAxis = true;
            }
        }

        //距離が近い状態が続いている場合は秒数をカウントする
        if (targetDis <= 2.5f)
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
        */
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

        if (_dis >= 2.0f) return false;

        Vector3 _fowardVec = _dog.transform.forward;
        _fowardVec.y = 0;
        _targetVec.y = 0;

        float _dot = Vector3.Dot(_targetVec.normalized, _fowardVec.normalized);

        if (Mathf.Acos(_dot) * Mathf.Rad2Deg >= 10.0f) return false;

        return true;
    }
}
