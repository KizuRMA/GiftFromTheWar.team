using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttackState : State<BatPatrolState>
{
    public BatAttackState(BatPatrolState owner) : base(owner) { }
    GameObject bat;
    bool switchAnime;
    bool facingThePlayer;

    public override void Enter()
    {
        bat = owner.bat;
        owner.agent.isStopped = true;
        facingThePlayer = false;

        owner.agent.destination = owner.bat.transform.position;
        owner.animator.SetFloat("AnimationSpeed", 1.1f);
    }

    public override void Execute()
    {
        if (facingThePlayer == false)
        {
            TurnToPlayer();
            if (facingThePlayer == true)
            {
                switchAnime = true;
                if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false) switchAnime = false;
                owner.animator.SetTrigger("Attack");
            }
            return;
        }

        //アニメーションが切り替わっていない場合
        if (switchAnime == false)
        {
            CheckSwitchAnime();
            return;
        }

        //アニメーションが終了している場合
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false)
        {
            owner.ChangeState(e_BatPatrolState.Tracking);
            return;
        }
    }

    public override void Exit()
    {
        owner.agent.isStopped = false;
    }

    void CheckSwitchAnime()
    {
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == true)
        {
            switchAnime = true;
        }
    }

    private void TurnToPlayer()
    {
        Vector3 _forwardVec = bat.transform.forward;
        _forwardVec.y = 0;
        _forwardVec = _forwardVec.normalized;
        Vector3 _targetVec = owner.player.transform.position - bat.transform.position;
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

        float _rotSpeed = 180.0f * Time.deltaTime;

        if (_degAng <= _rotSpeed)
        {
            _rotSpeed = _degAng;
            facingThePlayer = true;
        }

        Vector3 _localAngle;

        if (_forwardVec.x < 0)
        {
            _localAngle = bat.transform.localEulerAngles;
            _localAngle.y += _rotSpeed;
            bat.transform.localEulerAngles = _localAngle;
        }
        else
        {
            _localAngle = bat.transform.localEulerAngles;
            _localAngle.y -= _rotSpeed;
            bat.transform.localEulerAngles = _localAngle;
        }
    }
}
