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

    float loseSightOfDis;

    private bool IsTracking //追跡を続ける場合の前提条件
    {
        get
        {
            if (owner.transform.parent == null ||
                Vector3.Distance(owner.transform.position,owner.player.transform.position) >= loseSightOfDis)
            {
                return false;
            }

            return true;
        }
    }
    public override void Enter()
    {
        loseSightOfDis = owner.loseSightOfDis;
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

        //現在地から最も近いWayPointをターゲット座標にする
        agent.CalculatePath(owner.player.transform.position, navMeshPath);
        navController.Move(navMeshPath);
        owner.transform.position = new Vector3(owner.transform.position.x,agent.destination.y,owner.transform.position.z);

        float targetDis = Vector3.Distance(owner.dog.transform.position, owner.player.transform.position);

        //攻撃する条件
        if (IsPossibleToAttack() == true)
        {
            owner.ChangeState(e_DogState.Attack);
            return;
        }

        //見失う条件
        if (IsTraking() == true)
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


    private bool IsTraking()    //犬が追跡するか
    {
        //親オブジェクトがあるか確認する
        if (IsTracking) return false;

        DogManager _dogManager = owner.transform.parent.GetComponent<DogManager>();
        GameObject[] _objects = _dogManager.GetEnemys();

        //追跡している一番近い犬を取得する
        float _minDis = float.MaxValue;

        foreach (var objs in _objects)
        {
            //自分とは判定を行わない
            if (owner.gameObject == objs) continue;

            DogState _state = objs.GetComponent<DogState>();
            if (_state == null) continue;

            if (_state.IsChasing() == true)
            {
                float _dis = Vector3.Distance(owner.player.transform.position,_state.transform.position);
                if (_dis <= _minDis)
                {
                    _minDis = _dis;
                }
            }
        }

        //別の犬とプレイヤーの距離が見失う距離よりも小さい場合
        if (_minDis <= loseSightOfDis) return false;

        return true;
    }
}
