using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum e_DogState
{
    Search,
    Traking,
}

public class DogState : StatefulObjectBase<DogState, e_DogState>
{
    [SerializeField]public NavMeshAgent agent;
    [SerializeField]public GameObject player;
    [SerializeField]public GameObject dog;
    [SerializeField]public Animator animator;

    //SerchState
    [SerializeField] public float SearchSpeed;
    [SerializeField] public float TrakingSpeed;

    void Start()
    {
        stateList.Add(new DogSearchState(this));
        stateList.Add(new DogTrakingState(this));

        stateMachine = new StateMachine<DogState>();

        ChangeState(e_DogState.Search);
    }
}