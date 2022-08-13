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
    Wait,
}

public class DogState : StatefulObjectBase<DogState, e_DogState>
{
    //[Header("移動")]

    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public CharacterController controller;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject dog;
    [SerializeField] public Animator animator;

    [Header("犬のステータスパラメータ")]
    [TooltipAttribute("捜索する時の速度"), SerializeField] public float SearchSpeed;
    [TooltipAttribute("追跡する時の速度"),SerializeField] public float TrackingSpeed;
    [TooltipAttribute("攻撃の予備動作中の回転速度"),SerializeField] public float attackRotSpeed;
    [TooltipAttribute("最大ヒットポイント数"),SerializeField] public float life = 1.0f;
    [TooltipAttribute("プレイヤーを見失う時の距離"),SerializeField] public float loseSightOfDis;
    [TooltipAttribute("攻撃する時のジャンプの力の割合"),SerializeField] public float attackJumpPow = 1.0f;

    [System.NonSerialized] public Vector3 hypocenter;
    [System.NonSerialized] public bool canVigilance;
    [System.NonSerialized] public DogAttackFunction info;
    [System.NonSerialized] public DogTerritory territory;

    public bool IsVigilance => canVigilance == true;
    public bool IsAlive => life > 0.0f;

    public Vector3 startPos;

    void Start()
    {
        info = transform.GetComponent<DogAttackFunction>();

        startPos = transform.position;

        stateList.Add(new DogSearchState(this));
        stateList.Add(new DogTrackingState(this));
        stateList.Add(new DogVigilanceState(this));
        stateList.Add(new DogAttackState(this));
        stateList.Add(new DogCheckAroundState(this));
        stateList.Add(new DogMagnetCatchState(this));
        stateList.Add(new DogBlowedAwayState(this));
        stateList.Add(new DogWaitState(this));

        stateMachine = new StateMachine<DogState>();

        ChangeState(e_DogState.Wait);

        canVigilance = true;

      
    }

    protected override void Update()
    {
        base.Update();

        if (territory.isPlayerJoin == false)
        {
            if (IsCurrentState(e_DogState.Search) == true)
            {
                ChangeState(e_DogState.Wait);
            }
        }
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
        territory = _info.territory;
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

    public void SetStartPos(Vector3 _startPos)
    {
        startPos = _startPos;
    }
}
