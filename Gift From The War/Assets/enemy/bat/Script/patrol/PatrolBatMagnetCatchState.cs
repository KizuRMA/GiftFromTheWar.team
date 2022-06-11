using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBatMagnetCatchState : State<BatPatrolState>
{
    public PatrolBatMagnetCatchState(BatPatrolState owner) : base(owner) { }
    private Rigidbody rd;
    bool magnetCatchFlg;

    public override void Enter()
    {
        //この関数が実行された際にまだマグネットで捕まっていない時
        if (owner.transform.parent == null)
        {
            //マグネットで捕まっていないことを保存する
            magnetCatchFlg = false;
        }

        //ナビメッシュを切る
        owner.agent.isStopped = true;
        owner.agent.updatePosition = false;
        owner.agent.updateUpAxis = false;

        rd = owner.GetComponent<Rigidbody>();
        rd.isKinematic = false;
        rd.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public override void Execute()
    {
        if (magnetCatchFlg == false)
        {
            if (IsMagnetCatch() == true)
            {
                magnetCatchFlg = true;
            }
            else
            {
                return;
            }
        }

        //開放された時
        if (owner.transform.parent == null)
        {
            //Quaternion rotate = Quaternion.Euler(0, owner.transform.rotation.eulerAngles.y, 0);
            //owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, rotate, 60.0f * Time.deltaTime);
            //Vector3 dif = owner.transform.rotation.eulerAngles - rotate.eulerAngles;

            //if (dif.magnitude <= 0)
            //{
            //    owner.ChangeState(e_DogState.Tracking);
            //    return;
            //}
        }
    }

    public override void Exit()
    {

    }
    private bool IsMagnetCatch()
    {
        return owner.transform.parent != null;
    }
}
