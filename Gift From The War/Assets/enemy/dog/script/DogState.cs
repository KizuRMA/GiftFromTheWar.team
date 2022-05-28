using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum e_DogState
{
    Search,
    Traking,
    Vigilance,
    Attack,
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

    public bool canVigilance;
    public bool endAnimationFlg;

    public bool IsVigilance => canVigilance == true;

    void Start()
    {
        stateList.Add(new DogSearchState(this));
        stateList.Add(new DogTrakingState(this));
        stateList.Add(new DogVigilanceState(this));
        stateList.Add(new DogAttackState(this));

        stateMachine = new StateMachine<DogState>();

        ChangeState(e_DogState.Search);

        endAnimationFlg = false;
        canVigilance = true;
    }

    public void EndAnimation()
    {
        endAnimationFlg = true;
    }

    public IEnumerator CoolDownCoroutine()
    {
        canVigilance = false;
        yield return new WaitForSeconds(3.0f);
        canVigilance = true;
    }
}
