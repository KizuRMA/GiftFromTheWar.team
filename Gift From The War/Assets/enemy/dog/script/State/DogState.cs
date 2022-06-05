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
    [SerializeField]public GameObject player;
    [SerializeField]public GameObject dog;
    [SerializeField] public Animator animator;
    [SerializeField] private Collider attackCollider;
    [SerializeField] public float SearchSpeed;
    [SerializeField] public float TrakingSpeed;

    public bool canVigilance;
    public bool IsVigilance => canVigilance == true;

    void Start()
    {
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

    public void OnHitAttack(Collider _collider) //�U�������������Ƃ��̏���
    {
        var target = _collider.GetComponent<playerAbnormalcondition>();
        if (null == target) return;

        target.Damage(1.0f);
    }

    public void OnAttackStart()
    {
        attackCollider.enabled = true;
    }

    public void OnAttackFinished()
    {
        attackCollider.enabled = false;
    }

}
