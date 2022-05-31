using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum e_DogState
{
    Search,
    Tracking,
    Vigilance,
    Attack,
    CheckAround,
}

public class DogState : StatefulObjectBase<DogState, e_DogState>
{
    [SerializeField]public NavMeshAgent agent;
    [SerializeField] public NavMeshSurface[] navMeshSurface;
    [SerializeField]public GameObject player;
    [SerializeField]public GameObject dog;
    [SerializeField] public Animator animator;
    [SerializeField] public float SearchSpeed;
    [SerializeField] public float TrakingSpeed;

    public bool canVigilance;
    public bool IsVigilance => canVigilance == true;

    void Start()
    {
        navMeshSurface = GameObject.Find("stage").GetComponents<NavMeshSurface>();

        Debug.Log(NavMesh.GetSettingsNameFromID(navMeshSurface[0].agentTypeID));
        Debug.Log(NavMesh.GetSettingsNameFromID(navMeshSurface[1].agentTypeID));

        stateList.Add(new DogSearchState(this));
        stateList.Add(new DogTrackingState(this));
        stateList.Add(new DogVigilanceState(this));
        stateList.Add(new DogAttackState(this));
        stateList.Add(new DogCheckAroundState(this));

        stateMachine = new StateMachine<DogState>();

        ChangeState(e_DogState.Search);

        canVigilance = true;
    }

    public void EndAnimation()
    {

    }

    public IEnumerator CoolDownCoroutine()
    {
        canVigilance = false;
        yield return new WaitForSeconds(3.0f);
        canVigilance = true;
    }
}
