using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttackScript : BaseState
{
    [SerializeField] private Collider attackCollider;
    [SerializeField] private float attackCooldown = 0.5f;

    // Start is called before the first frame update

    private void Awake()
    {
        myController = GetComponent<BatController>();
    }

    public override void Update()
    {
        //体を前に傾ける
        Vector3 _localAngle = transform.localEulerAngles;
        _localAngle.x = myController.forwardAngle;
        transform.localEulerAngles = _localAngle;

        //高さを調整する
        myController.AdjustHeight();
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
       AttackIfPossible();
    }

    public void OnAttackStart()
    {
        attackCollider.enabled = true;
    }

    public void OnHitAttack(Collider _collider)
    {

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
