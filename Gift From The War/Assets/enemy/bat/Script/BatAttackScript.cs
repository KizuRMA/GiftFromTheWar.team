using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatAttackScript : BaseState
{
    [SerializeField] private Collider attackCollider;
    [SerializeField] private float attackCooldown = 0.5f;
    NavMeshAgent agent;
    GameObject player;
    public bool stateChangeFlg;

    // Start is called before the first frame update

    private void Awake()
    {
        myController = GetComponent<BatController>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("player");
        CurrentState = (int)BatController.e_State.attack;
    }

    public override void Start()
    {
        playerAbnormalcondition abnormalcondition = player.GetComponent<playerAbnormalcondition>();
        abnormalcondition.AddHowlingAbnormal();
        base.Start();
        stateChangeFlg = false;
        //agent.isStopped = false;
    }

    public override void Update()
    {
        //‘Ì‚ð‘O‚ÉŒX‚¯‚é
        Vector3 _localAngle = transform.localEulerAngles;
        _localAngle.x = myController.forwardAngle;
        transform.localEulerAngles = _localAngle;


    }

    public void AttackIfPossible()
    {
        if (!myController.IsAttackable) return;

        myController.ChangeState(this);
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Attack");
    }

    public void OnAttackRangeEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            AttackIfPossible();

            myController.OffNavMesh();
            agent.destination = transform.position;
        }
    }

    public void OnAttackStart()
    {
        attackCollider.enabled = true;
    }

    public void OnHitAttack(Collider _collider)
    {
        var target = _collider.GetComponent<playerAbnormalcondition>();
        if (null == target) return;

        target.Damage(1);
    }

    public void OnAttackFinished()
    {
        attackCollider.enabled = false;
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        myController.ChangeState(GetComponent<batMove>());
    }
}
