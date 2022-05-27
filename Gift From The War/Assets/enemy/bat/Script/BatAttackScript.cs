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

        myController.OffNavMesh();
        ChangeUltrasound(GetComponent<SmallUltrasound>());
        ultrasound.Start();
    }

    public override void Update()
    {
        RotateUpdate();
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
        if (collider.tag != "Player") return;
        AttackIfPossible();

        agent.updatePosition = false;
        agent.destination = transform.position;
    }

    public void OnAttackStart()
    {
        attackCollider.enabled = true;
    }

    public void OnHitAttack(Collider _collider)
    {
        var target = _collider.GetComponent<playerAbnormalcondition>();
        if (null == target) return;

        target.Damage(1.0f);
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

    private void RotateUpdate()
    {
        //‘Ì‚ð‘O‚ÉŒX‚¯‚é
        Vector3 _localAngle = transform.localEulerAngles;
        _localAngle.x = myController.forwardAngle;
        transform.localEulerAngles = _localAngle;

        Vector3 _forwardVec = transform.forward;
        _forwardVec.y = 0;
        _forwardVec = _forwardVec.normalized;
        Vector3 _targetVec = player.transform.position - transform.position;
        _targetVec.y = 0;
        _targetVec = _targetVec.normalized;

        float dot = Vector3.Dot(_targetVec, Vector3.forward);
        float _degAng = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (_targetVec.x < 0)
        {
            _degAng *= -1.0f;
        }

        _forwardVec = Quaternion.Euler(0, -_degAng, 0) * _forwardVec;
        _targetVec = Quaternion.Euler(0, -_degAng, 0) * _targetVec;

        dot = Vector3.Dot(_targetVec, _forwardVec);
        _degAng = Mathf.Acos(dot) * Mathf.Rad2Deg;

        float _rotSpeed = 120.0f * Time.deltaTime;
        if (_degAng <= _rotSpeed)
        {
            _rotSpeed = _degAng;
        }

        if (_forwardVec.x < 0)
        {
            _localAngle = transform.localEulerAngles;
            _localAngle.y += _rotSpeed;
            transform.localEulerAngles = _localAngle;
        }
        else
        {
            _localAngle = transform.localEulerAngles;
            _localAngle.y -= _rotSpeed;
            transform.localEulerAngles = _localAngle;
        }
    }
}
