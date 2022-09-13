using UnityEngine;
using UnityEngine.AI;

public class BatShakeHeadState : State<BatPatrolState>
{
    public BatShakeHeadState(BatPatrolState owner) : base(owner) { }
    bool switchAnime;
    bool movement;
    float time;

    public float DistanceXZ(Vector3 src, Vector3 dst)
    {
        src.y = dst.y;
        return Vector3.Distance(src, dst);
    }

    public override void Enter()
    {
        time = 0;
        movement = true;
        owner.animator.SetFloat("AnimationSpeed", 1.0f);
        owner.agent.speed = owner.moveWayPointSpeed;
    }

    public override void Execute()
    {
        //移動している場合
        if (movement == true)
        {
            time += Time.deltaTime;
            NavMeshAgent _agent = owner.agent;

            float targetDis = DistanceXZ(owner.transform.position,_agent.destination);

            if (targetDis >= 15.0f)
            {
                //アニメーションを切り替える
                switchAnime = true;
                if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("ShakeHead") == false) switchAnime = false;
                owner.animator.SetTrigger("ShakeHead");
                movement = false;
                return;
            }

            //プレイヤーの消息が切れた座標に付いた場合
            if (targetDis <= 0.3f || time >= 20.0f)
            {
                //アニメーションを切り替える
                switchAnime = true;
                if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("ShakeHead") == false) switchAnime = false;
                owner.animator.SetTrigger("ShakeHead");
                movement = false;
                return;
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
