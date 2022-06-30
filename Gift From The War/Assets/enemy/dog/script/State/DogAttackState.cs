using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogAttackState : State<DogState>
{
    public DogAttackState(DogState owner) : base(owner) { }
    private DogAttackFunction info;
    private Rigidbody rd;
    private GameObject myGame;
    bool switchAnime;

    public override void Enter()
    {
        myGame = owner.dog;
        info = owner.info;

        //アニメーションを変化
        owner.animator.SetInteger("trans", 0);
        owner.animator.SetFloat("Speed", 1.0f);
        owner.animator.SetTrigger("Attack");
        switchAnime = true;
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false) switchAnime = false;

        rd = owner.dog.GetComponent<Rigidbody>();
        owner.agent.isStopped = true;
        owner.agent.updatePosition = false;
        owner.agent.updateUpAxis = false;

        rd.isKinematic = false;
        rd.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public override void Execute()
    {
        //アニメーションが切り替わっていない場合
        if (switchAnime == false)
        {
            CheckSwitchAnime();
            return;
        }

        if (info.isJumpFlg == false)
        {
            TurnToPlayer();
        }

        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            owner.ChangeState(e_DogState.Tracking);
            return;
        }
    }

    public override void Exit()
    {
        owner.agent.isStopped = false;
        owner.agent.updatePosition = true;

        owner.agent.updateUpAxis = true;

        owner.agent.Warp(owner.dog.transform.position);

        rd.constraints = RigidbodyConstraints.None;
        rd.isKinematic = true;
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
        Vector3 _forwardVec = myGame.transform.forward;
        _forwardVec.y = 0;
        _forwardVec = _forwardVec.normalized;
        Vector3 _targetVec = owner.player.transform.position - myGame.transform.position;
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

        float _rotSpeed = 360.0f * Time.deltaTime;

        if (_degAng <= _rotSpeed)
        {
            _rotSpeed = _degAng;
        }

        Vector3 _localAngle;

        if (_forwardVec.x < 0)
        {
            _localAngle = myGame.transform.localEulerAngles;
            _localAngle.y += _rotSpeed;
            myGame.transform.localEulerAngles = _localAngle;
        }
        else
        {
            _localAngle = myGame.transform.localEulerAngles;
            _localAngle.y -= _rotSpeed;
            myGame.transform.localEulerAngles = _localAngle;
        }
    }
}
