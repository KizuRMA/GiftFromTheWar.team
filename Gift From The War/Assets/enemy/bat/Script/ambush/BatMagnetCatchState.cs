using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatMagnetCatchState : BaseState
{
    private Rigidbody rd;
    bool magnetCatchFlg;
    bool releaseFlg;

    public override void Start()
    {
        myController = GetComponent<BatController>();
        rd = transform.GetComponent<Rigidbody>();
        CurrentState = (int)BatController.e_State.magnetCatch;

        //�i�r���b�V����؂�
        myController.agent.isStopped = true;
        myController.agent.updatePosition = false;
        myController.agent.updateUpAxis = false;

        rd.isKinematic = false;
        rd.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        //���̊֐������s���ꂽ�ۂɂ܂��}�O�l�b�g�ŕ߂܂��Ă��Ȃ���
        if (transform.parent == null)
        {
            //�}�O�l�b�g�ŕ߂܂��Ă��Ȃ����Ƃ�ۑ�����
            magnetCatchFlg = false;
        }

        releaseFlg = false;
    }

    // Update is called once per frame
    public override void Update()
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
        if (myController.transform.parent == null)
        {
            if (releaseFlg == false)
            {
                //������ꂽ���x�������s����
                ReleasedOnce();
                releaseFlg = true;
            }

            Quaternion rotate = Quaternion.Euler(myController.forwardAngle, transform.rotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, 60.0f * Time.deltaTime);
            Vector3 dif = transform.rotation.eulerAngles - rotate.eulerAngles;


            if (dif.magnitude <= 0.2f)
            {
                transform.rotation = rotate;
                //myController.ChangeState();
                return;
            }
        }
    }

    public override void Exit()
    {

    }

    private bool IsMagnetCatch()
    {
        return transform.parent != null;
    }

    private void ReleasedOnce()
    {
        LayerMask layer = transform.GetComponent<MoveNavMeshLink>().raycastLayerMask;

        //�i�r���b�V���̉e����Y���̒l���n�ʂ̍��W�ɂȂ��Ă���
        Ray _ray = new Ray(transform.position, Vector3.down);
        RaycastHit _raycastHit;
        bool _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, layer);

        myController.height = _raycastHit.distance;

        rd.useGravity = false;
        rd.isKinematic = true;
    }
}
