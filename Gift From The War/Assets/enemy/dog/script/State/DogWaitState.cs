using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogWaitState : State<DogState>
{
    struct NavMeshParameter
    {
        public float speed;
        public float angularSpeed;
        public float acceleration;
    }

    public DogWaitState(DogState owner) : base(owner) { }

    public NavMeshAgent agent;
    public CharacterController controller;
    public NavController navController;
    NavMeshParameter agentParameter;

    bool switchAnime;

    public override void Enter()
    {
        navController = owner.transform.GetComponent<NavController>();
        agent = owner.agent;
        controller = owner.controller;

        owner.animator.Play("metarig|action_Run");
        owner.animator.SetFloat("Speed", 1.1f);

        //NavMeshAgentÇÃÉpÉâÉÅÅ[É^Çï€ë∂ÇµÇƒÇ®Ç≠
        agentParameter.speed = agent.speed;
        agentParameter.angularSpeed = agent.angularSpeed;
        agentParameter.acceleration = agent.acceleration;

        agent.speed = 0f;
        agent.angularSpeed = 0f;
        agent.acceleration = 0f;

        controller.enabled = true;
        switchAnime = false;
    }

    public override void Execute()
    {
        owner.animator.SetFloat("MoveSpeed", 1.0f);
        agent.destination = owner.startPos;

        NavMeshPath navMeshPath = new NavMeshPath();

        agent.CalculatePath(owner.startPos, navMeshPath);
        navController.Move(navMeshPath);
        owner.transform.position = new Vector3(owner.transform.position.x, agent.destination.y, owner.transform.position.z);

        float _targetDis = Vector2.Distance(new Vector2(owner.startPos.x, owner.startPos.z),
                                            new Vector2(owner.transform.position.x, owner.transform.position.z));

        if (_targetDis <= 1.0f)
        {
            if (switchAnime == false)
            {
                owner.animator.SetInteger("trans", 0);
                switchAnime = true;
            }

            if (owner.territory.isPlayerJoin == true)
            {
                owner.ChangeState(e_DogState.Search);
            }
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
}
