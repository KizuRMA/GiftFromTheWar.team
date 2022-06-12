using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBatMagnetCatchState : State<BatPatrolState>
{
    public PatrolBatMagnetCatchState(BatPatrolState owner) : base(owner) { }
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

            Quaternion rotate = Quaternion.Euler(owner.forwardAngle, owner.transform.rotation.eulerAngles.y, 0);
            owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, rotate, 60.0f * Time.deltaTime);
            Vector3 dif = owner.transform.rotation.eulerAngles - rotate.eulerAngles;


            if (dif.magnitude <= 0.2f)
            {
                owner.transform.rotation = rotate;
                owner.ChangeState(e_BatPatrolState.Tracking);
                return;
            }
        }
    }

    public override void Exit()
    {
        owner.agent.isStopped = false;
        owner.agent.updatePosition = true;
        owner.agent.updateUpAxis = true;

        owner.agent.Warp(owner.transform.position);

        rd.constraints = RigidbodyConstraints.None;
    }
    private bool IsMagnetCatch()
    {
        return owner.transform.parent != null;
    }

    private void ReleasedOnce()
    {
        LayerMask layer = owner.transform.GetComponent<PatrolBatMoveNavMeshLink>().raycastLayerMask;
        
        //�i�r���b�V���̉e����Y���̒l���n�ʂ̍��W�ɂȂ��Ă���
        Ray _ray = new Ray(owner.transform.position, Vector3.down);
        RaycastHit _raycastHit;
        bool _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, layer);

        owner.height = _raycastHit.distance;

        rd.useGravity = false;
        rd.isKinematic = true;
    }
}
