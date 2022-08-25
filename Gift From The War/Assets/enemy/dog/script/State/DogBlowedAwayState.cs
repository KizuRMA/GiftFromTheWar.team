using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBlowedAwayState : State<DogState>
{
    public DogBlowedAwayState(DogState owner) : base(owner) { }
    private Rigidbody rd;

    public override void Enter()
    {
        owner.animator.Play("metarig|action_Sniff");
        owner.animator.speed = 0.5f;

        //ナビメッシュを切る
        owner.agent.isStopped = true;
        owner.agent.updatePosition = false;
        owner.agent.updateUpAxis = false;

        rd = owner.GetComponent<Rigidbody>();
        rd.isKinematic = false;


        Vector3 _dirVec = (owner.transform.position + new Vector3(0, 1.0f, 0)) - owner.hypocenter;
        float dis = _dirVec.magnitude;
        _dirVec = _dirVec.normalized * (8.0f - Mathf.Min(dis,0.5f) * 5);

        Vector3 _torque = -owner.transform.right;

        rd.AddForce(_dirVec, ForceMode.Impulse);
        rd.AddTorque(Quaternion.Euler(0, 90.0f, 0) * _dirVec, ForceMode.Impulse);

        owner.gameObject.AddComponent<MeshRenderer>();
        owner.gameObject.AddComponent<NotSeeObjectDelete>();

        if (owner.button != null)
        {
            owner.button.transform.parent = null;
            Rigidbody _rd = owner.button.GetComponent<Rigidbody>();

            if (_rd == null)_rd = owner.button.AddComponent<Rigidbody>();
            _rd.useGravity = true;
        }
    }



    public override void Execute()
    {
        //アニメーション速度を徐々に落とす
        if (owner.animator.speed > 0)
        {
            float difAmount = Time.deltaTime * 0.2f;
            owner.animator.speed -= difAmount;
            if (owner.animator.speed < difAmount)
            {
                owner.animator.speed = 0;
            }
        }
    }

    public override void Exit()
    {

    }
}
