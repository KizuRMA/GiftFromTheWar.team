using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMagnetCatchState : State<DogState>
{
    public DogMagnetCatchState(DogState owner) : base(owner) { }
    private Rigidbody rd;
    bool magnetCatchFlg;
    bool releaseFlg;

    public override void Enter()
    {
        //���̊֐������s���ꂽ�ۂɂ܂��}�O�l�b�g�ŕ߂܂��Ă��Ȃ���
        if (owner.transform.parent == null)
        {
            //�}�O�l�b�g�ŕ߂܂��Ă��Ȃ����Ƃ�ۑ�����
            magnetCatchFlg = false;
        }

        releaseFlg = false;

        //�i�r���b�V����؂�
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

        //�J�����ꂽ��
        if (owner.transform.parent == null)
        {
            if (releaseFlg == false)
            {
                //������ꂽ���x�������s����
                ReleasedOnce();
                releaseFlg = true;
            }

            Quaternion rotate = Quaternion.Euler(0,owner.transform.rotation.eulerAngles.y,0);
            owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, rotate, 60.0f * Time.deltaTime);
            Vector3 dif = owner.transform.rotation.eulerAngles - rotate.eulerAngles;

            if (dif.magnitude <= 0 && rd.velocity.y >= 0)
            {
                owner.ChangeState(e_DogState.Tracking);
                return;
            }
        }
    }

    public override void Exit()
    {
        owner.SetParentManager();
        owner.agent.isStopped = false;
        owner.agent.updatePosition = true;
        owner.agent.updateUpAxis = true;

        owner.agent.Warp(owner.dog.transform.position);

        rd.isKinematic = true;
        rd.constraints = RigidbodyConstraints.None;
    }

    private bool IsMagnetCatch()
    {
        return owner.transform.parent != null;
    }

    private void ReleasedOnce()
    {

    }
}
