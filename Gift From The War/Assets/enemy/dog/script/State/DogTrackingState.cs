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

    private bool IsTracking //�ǐՂ𑱂���ꍇ�̑O�����
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

        //���ݒn����ł��߂�WayPoint���^�[�Q�b�g���W�ɂ���
        agent.CalculatePath(owner.player.transform.position, navMeshPath);
        navController.Move(navMeshPath);
        owner.transform.position = new Vector3(owner.transform.position.x,agent.destination.y,owner.transform.position.z);

        float targetDis = Vector3.Distance(owner.dog.transform.position, owner.player.transform.position);

        //�U���������
        if (IsPossibleToAttack() == true)
        {
            owner.ChangeState(e_DogState.Attack);
            return;
        }

        //����������
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


    private bool IsTraking()    //�����ǐՂ��邩
    {
        //�e�I�u�W�F�N�g�����邩�m�F����
        if (IsTracking) return false;

        DogManager _dogManager = owner.transform.parent.GetComponent<DogManager>();
        GameObject[] _objects = _dogManager.GetEnemys();

        //�ǐՂ��Ă����ԋ߂������擾����
        float _minDis = float.MaxValue;

        foreach (var objs in _objects)
        {
            //�����Ƃ͔�����s��Ȃ�
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

        //�ʂ̌��ƃv���C���[�̋��������������������������ꍇ
        if (_minDis <= loseSightOfDis) return false;

        return true;
    }
}
