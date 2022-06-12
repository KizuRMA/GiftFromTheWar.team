using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatMagnetCatchState : BaseState
{
    [SerializeField] private Rigidbody rd;
    bool magnetCatchFlg;
    bool releaseFlg;
    bool foldWingFlg;
    bool aliveFlg;
    float rotateY;

    public override void Start()
    {
        myController = GetComponent<BatController>();
        CurrentState = (int)BatController.e_State.magnetCatch;

        //ナビメッシュを切る
        myController.agent.isStopped = true;
        myController.agent.updatePosition = false;
        myController.agent.updateUpAxis = false;


        GetComponent<BoxCollider>().isTrigger = false;
        rd.isKinematic = false;
        rd.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        //この関数が実行された際にまだマグネットで捕まっていない時
        if (transform.parent == null)
        {
            //マグネットで捕まっていないことを保存する
            magnetCatchFlg = false;
        }
        foldWingFlg = false;
        releaseFlg = false;
        aliveFlg = true;
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

        //開放された時
        if (myController.transform.parent == null)
        {
            if (releaseFlg == false)
            {
                //解放された後一度だけ実行する
                ReleasedOnce();
                releaseFlg = true;
            }


            if (foldWingFlg == true)
            {
                if (rd.velocity.y <= -6.0f)
                {
                    aliveFlg = false;
                }

                //羽を閉じている状態での回転
                FoldWindRotateUpdate();
            }
            else
            {
                Quaternion rotate = Quaternion.Euler(myController.forwardAngle, transform.rotation.eulerAngles.y, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, 60.0f * Time.deltaTime);
                Vector3 dif = transform.rotation.eulerAngles - rotate.eulerAngles;

                if (dif.magnitude <= 0.2f)
                {
                    transform.rotation = rotate;
                    myController.ChangeState(transform.GetComponent<batMove>());
                    return;
                }
            }
        }
    }

    public override void Exit()
    {
        myController.agent.isStopped = false;
        myController.agent.updatePosition = true;
        myController.agent.updateUpAxis = true;

        myController.agent.Warp(transform.position);

        rd.constraints = RigidbodyConstraints.None;

        rd.useGravity = false;
        rd.isKinematic = true;
    }

    private bool IsMagnetCatch()
    {
        return transform.parent != null;
    }

    private void ReleasedOnce()
    {
        if (myController.animator.GetCurrentAnimatorStateInfo(0).IsName("FoldTheWings") == true)
        {
            foldWingFlg = true;
            rd.velocity = new Vector3(0,-0.1f,0);
            rotateY = transform.eulerAngles.y;
        }
        else
        {
            HeightUpdate();
            rd.useGravity = false;
            rd.isKinematic = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rd.velocity.y >= 0 || releaseFlg == false) return;

        if (aliveFlg == false)
        {
            HeightUpdate();
            myController.ChangeState(transform.GetComponent<DeadState>());
        }
    }

    private void FoldWindRotateUpdate()
    {
        //コウモリの回転処理
        if (myController.forwardAngle > 20.0f)
        {
            //変化する値
            float _changeAng = 90.0f * (Time.deltaTime * 1.5f);
            myController.forwardAngle -= _changeAng;

            if (myController.forwardAngle <= 20.0f)
            {
                myController.forwardAngle = 20.0f;
                myController.animator.SetInteger("trans", 0);
                myController.animator.SetFloat("AnimationSpeed", 1.3f);
                myController.animator.Play("FlappingWings");
                HeightUpdate();
                myController.ChangeState(transform.GetComponent<batMove>());
            }
        }

        //体を回転させる処理
        if (myController.forwardAngle >= 90)
        {
            transform.eulerAngles = new Vector3(180.0f - myController.forwardAngle, rotateY + 180.0f, 180.0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(myController.forwardAngle, rotateY, 0);
        }
    }

    private void HeightUpdate()
    {
        LayerMask layer = transform.GetComponent<MoveNavMeshLink>().raycastLayerMask;

        //ナビメッシュの影響でY軸の値が地面の座標になっている
        Ray _ray = new Ray(transform.position, Vector3.down);
        RaycastHit _raycastHit;
        bool _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, layer);

        myController.height = _raycastHit.distance;
    }
}
