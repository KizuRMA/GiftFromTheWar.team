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
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject dog;
    [SerializeField] public Animator animator;
    [SerializeField] public float SearchSpeed;
    [SerializeField] public float TrakingSpeed;
    [SerializeField] public float life = 1.0f;
    [SerializeField] public float attackJumpPow = 1.0f;

    [System.NonSerialized] public Vector3 hypocenter;
    [System.NonSerialized] public bool canVigilance;
    [System.NonSerialized] public DogAttackFunction info;

    public bool IsVigilance => canVigilance == true;
    public bool IsAlive => life > 0.0f;

    void Start()
    {
        info = transform.GetComponent<DogAttackFunction>();

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

    //プレハブ化している場合に消えてしまうシリアライズの値を代入
    public void PutInInfo(EnemyManager _info)
    {
        player = _info.player;
        manager = _info.manager;
    }

    public void SetParentManager()  //マネージャーを親にする
    {
        transform.parent = manager.transform;
    }

    public bool IsChasing()
    {
        if (IsCurrentState(e_DogState.Attack) ||
            IsCurrentState(e_DogState.Tracking) ||
            IsCurrentState(e_DogState.MagnetCatch))
        {
            return true;
        }
        return false;
    }
}
