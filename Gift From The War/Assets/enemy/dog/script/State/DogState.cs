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
    MagnetCatch,
    BlowedAway,
}

public class DogState : StatefulObjectBase<DogState, e_DogState>
{
    [SerializeField]public NavMeshAgent agent;
    [SerializeField]public GameObject player;
    [SerializeField]public GameObject dog;
    [SerializeField] public Animator animator;
    [SerializeField] public float SearchSpeed;
    [SerializeField] public float TrakingSpeed;

    public Vector3 hypocenter;
    private Rigidbody rd;
    public bool canVigilance;
    public bool IsVigilance => canVigilance == true;

    public float life = 1.0f;
    public bool IsAlive => life > 0.0f;

    void Start()
    {
        stateList.Add(new DogSearchState(this));
        stateList.Add(new DogTrackingState(this));
        stateList.Add(new DogVigilanceState(this));
        stateList.Add(new DogAttackState(this));
        stateList.Add(new DogCheckAroundState(this));
        stateList.Add(new DogMagnetCatchState(this));
        stateList.Add(new DogBlowedAwayState(this));

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

    public void MagnetCatch()
    {
        if (IsAlive == false) return;
        ChangeState(e_DogState.MagnetCatch);
    }

    public void ExplosionHit(int _damage,Vector3 _hypocenter)
    {
        if (IsAlive == false) return;
        life -= _damage;

        hypocenter = _hypocenter;
        ChangeState(e_DogState.BlowedAway);
    }

    public void WarpPosition(Vector3 _pos)
    {
        agent.Warp(_pos);
        dog.transform.position = _pos;
    }
}
