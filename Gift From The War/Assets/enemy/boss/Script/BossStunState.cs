using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStunState : State<BossState>
{
    public BossStunState(BossState owner) : base(owner) { }
    bool switchAnime;

    public override void Enter()
    {
        owner.animator.SetTrigger("Stun");

        switchAnime = true;
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Stun") == false) switchAnime = false;

        owner.agent.isStopped = true;

        if (owner.keyObj != null)
        {
            //鍵を取ってない状態でボスがダメージを受けた時
            if (owner.keyObj.isGetKeyFlg == false)
            {
                owner.keyObj.KeyTakeFunction();
            }
        }

        AudioManager.Instance.PlaySE("BossGroaning", owner.gameObject, isLoop: false, vol: 1);

        AudioSource audio = owner.footstepsObj.GetComponent<AudioSource>();

        if (audio != null)
        {
            audio.Stop();
        }
    }

    public override void Execute()
    {
        //アニメーションが切り替わっていない場合
        if (switchAnime == false)
        {
            CheckSwitchAnime();
            return;
        }

        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            owner.ChangeState(e_BossState.Tracking);
            return;
        }
    }

    public override void Exit()
    {
        owner.agent.isStopped = false;
        //owner.agent.updatePosition = true;
        owner.WarpPosition(owner.transform.position);

        AudioSource audio = owner.footstepsObj.GetComponent<AudioSource>();

        if (audio != null)
        {
            audio.Play();
        }
    }

    void CheckSwitchAnime()
    {
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Stun") == true)
        {
            switchAnime = true;
        }
    }
}
