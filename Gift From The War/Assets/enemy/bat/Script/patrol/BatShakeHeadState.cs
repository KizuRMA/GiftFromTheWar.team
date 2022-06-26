using UnityEngine;
using UnityEngine.AI;

public class BatShakeHeadState : State<BatPatrolState>
{
    public BatShakeHeadState(BatPatrolState owner) : base(owner) { }
    bool switchAnime;
    bool movement;

    public override void Enter()
    {
        movement = true;
        owner.animator.SetFloat("AnimationSpeed", 1.0f);
        owner.agent.speed = owner.moveWayPointSpeed;
    }

    public override void Execute()
    {
        //�ړ����Ă���ꍇ
        if (movement == true)
        {
            NavMeshAgent _agent = owner.agent;

            Vector3 _nowPos = new Vector3(owner.bat.transform.position.x, _agent.destination.y, owner.bat.transform.position.z);
            float targetDis = Vector3.Distance(_nowPos, _agent.destination);

            if (targetDis >= 15.0f)
            {
                //�A�j���[�V������؂�ւ���
                switchAnime = true;
                if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("ShakeHead") == false) switchAnime = false;
                owner.animator.SetTrigger("ShakeHead");
                movement = false;
                return;
            }

            //�v���C���[�̏������؂ꂽ���W�ɕt�����ꍇ
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                //�A�j���[�V������؂�ւ���
                switchAnime = true;
                if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("ShakeHead") == false) switchAnime = false;
                owner.animator.SetTrigger("ShakeHead");
                movement = false;
                return;
            }

            return;
        }

        //�A�j���[�V�������؂�ւ���Ă��Ȃ��ꍇ
        if (switchAnime == false)
        {
            CheckSwitchAnime();
            return;
        }

        //�A�j���[�V�������I�����Ă���ꍇ
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("ShakeHead") == false)
        {
            owner.ChangeState(e_BatPatrolState.MoveWayPoints);
            return;
        }
    }

    public override void Exit()
    {

    }

    void CheckSwitchAnime()
    {
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("ShakeHead") == true)
        {
            switchAnime = true;
        }
    }
}
