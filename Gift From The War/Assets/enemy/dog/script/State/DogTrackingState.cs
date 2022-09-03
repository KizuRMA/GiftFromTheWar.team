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

    //�ǐՔ͈�
    float trackingRange;

    private bool IsTracking //�ǐՂ��邩
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

        //NavMeshAgent�̃p�����[�^��ۑ����Ă���
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

        //�U���������
        if (IsPossibleToAttack())
        {
            owner.ChangeState(e_DogState.Attack);
            return;
        }

        //����������
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


    private bool IsMiss()    //������
    {
        //�ǐՂ�������𖞂����Ă���Ȃ�
        if (IsTracking) return false;

        DogManager _dogManager = owner.transform.parent.GetComponent<DogManager>();
        GameObject[] _objects = _dogManager.GetEnemys();

        //�v���C���[��ǐՂ��A�ł��߂��ɂ��錢
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

        //���̌����ǐՔ͈͓��ɂ���Ȃ�
        if (_minDis <= trackingRange) return false;

        return true;
    }
}
