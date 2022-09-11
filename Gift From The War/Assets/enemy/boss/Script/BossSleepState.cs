using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSleepState : State<BossState>
{
    public BossSleepState(BossState owner) : base(owner) { }
    private GameObject voiceObj;
    float voiceTimer;

    public override void Enter()
    {
        owner.agent.isStopped = true;
        owner.agent.updatePosition = false;
        owner.agent.updateRotation = false;
        voiceTimer = 0;

        voiceObj = new GameObject("voice");
        voiceObj.transform.position = owner.transform.position;
    }

    public override void Execute()
    {
        if (owner.getupFlg == true)
        {
            voiceObj.SetActive(false);
            AudioManager.Instance.PlaySE("BossGetUpVoice", owner.transform.gameObject,vol:2.0f);
            owner.animator.SetTrigger("GetUp");
            owner.animator.SetFloat("Speed", 0.4f);
        }
        else
        {
            voiceTimer += Time.deltaTime;
            if (voiceTimer >= 12.0f)
            {
                AudioManager.Instance.PlaySE("BossSleepVoice", voiceObj, vol:0.5f);
                voiceTimer = 0;
            }
        }

        //起きるアニメーションに切り替わったとき
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") &&
            owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            AudioManager.Instance.PlaySE("BossFootsteps", owner.footstepsObj, isLoop: true, vol: 0.5f);
            owner.ChangeState(e_BossState.Wait);
        }
    }

    public override void Exit()
    {
        voiceObj.SetActive(false);

        owner.agent.isStopped = false;
        owner.agent.updatePosition = true;
        owner.agent.updateRotation = true;
    }
}
