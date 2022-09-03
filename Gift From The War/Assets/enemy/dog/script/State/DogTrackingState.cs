using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogTrackingState : State<DogState>
{
    struct NavMeshParameter
    {
        public float speed;
        public float angularSpeed;
        public float acceleration;
    }

    public DogTrackingState(DogState owner) : base(owner) { }
    public NavMeshAgent agent;
    public CharacterController controller;
    public NavController navController;
    NavMeshParameter agentParameter;

    //追跡範囲
    float trackingRange;

    private bool IsTracking //追跡するか
    {
        get
        {
            if (owner.transform.parent != null &&
                Vector3.Distance(owner.transform.position, owner.player.transform.position) <= trackingRange)
            {
                return true;
            }
            return false;
        }
    }
    public override void Enter()
    {
        trackingRange = owner.trackingRange;
        navController = owner.transform.GetComponent<NavController>();
        agent = owner.agent;
        controller = owner.controller;

        owner.animator.SetInteger("trans", 1);
        owner.animator.SetFloat("Speed", 1.1f);

        //NavMeshAgentのパラメータを保存しておく
        agentParameter.speed = agent.speed;
        agentParameter.angularSpeed = agent.angularSpeed;
        agentParameter.acceleration = agent.acceleration;

        agent.speed = 0f;
        agent.angularSpeed = 0f;
        agent.acceleration = 0f;

        controller.enabled = true;
    }

    public override void Execute()
    {


        owner.animator.SetFloat("MoveSpeed", 1.0f);
        agent.destination = owner.player.transform.position;

        NavMeshPath navMeshPath = new NavMeshPath();
        agent.CalculatePath(owner.player.transform.position, navMeshPath);

        float dis = 0.0f;

        Vector3 corner = owner.transform.position; ;
        for (int i = 0; i < navMeshPath.corners.Length; i++)
        {
            Vector3 corner2 = navMeshPath.corners[i];
            dis += Vector3.Distance(corner, corner2);
            corner = corner2;
        }

        navController.Move(navMeshPath, dis / 20.0f);
        owner.transform.position = new Vector3(owner.transform.position.x, agent.destination.y, owner.transform.position.z);

        //攻撃する条件
        if (IsPossibleToAttack())
        {
            owner.ChangeState(e_DogState.Attack);
            return;
        }

        //見逃す条件
        if (IsMiss())
        {
            owner.ChangeState(e_DogState.CheckAround);
            return;
        }
    }

    public override void Exit()
    {
        owner.animator.SetFloat("Speed", 1.0f);

        controller.enabled = false;
        agent.speed = agentParameter.speed;
        agent.angularSpeed = agentParameter.angularSpeed;
        agent.acceleration = agentParameter.acceleration;

        navController.Reset();
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


    private bool IsMiss()    //見失う
    {
        //追跡する条件を満たしているなら
        if (IsTracking) return false;

        DogManager _dogManager = owner.transform.parent.GetComponent<DogManager>();
        GameObject[] _objects = _dogManager.GetEnemys();

        //プレイヤーを追跡し、最も近くにいる犬
        float _minDis = float.MaxValue;

        foreach (var objs in _objects)
        {
            if (owner.gameObject == objs) continue;

            DogState _state = objs.GetComponent<DogState>();
            if (_state != null && _state.IsChasing() == false) continue;

            float _dis = Vector3.Distance(owner.player.transform.position, _state.transform.position);
            if (_dis <= _minDis)
            {
                _minDis = _dis;
            }
        }

        //他の犬が追跡範囲内にいるなら
        if (_minDis <= trackingRange) return false;

        return true;
    }
}
