using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHundLadder : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    [SerializeField] private CharacterController playerCC;
    [SerializeField] private GameObject playerCamera;
    private Transform playerTrans;
    private Transform cmaTrans;

    //フラグ関係
    public bool closeLadderFlg { get; set; }    //梯子の近くにいるかどうか
    public bool touchLadderFlg { get; set; }     //梯子に触れているか

    private bool moveBeforeFlg = false;     //梯子の前までいったか
    private bool rotXBeforeFlg = false;     //梯子の方に向いているか
    private bool rotYBeforeFlg = false;     //梯子の方に向いているか

    //梯子の情報
    private Vector3 ladderStartPos;     //梯子の始点の座標
    private Vector3 ladderStartRot;     //梯子の向き
    private Quaternion ladderStartQua;  //梯子の向き
    private Vector3 ladderEndPos;       //梯子の終点の座標

    //梯子までの移動
    [SerializeField] private float moveBeforeSpeed;   //梯子の前まで行く速さ
    [SerializeField] private float rotXBeforeSpeed;   //X軸回転の速さ
    [SerializeField] private float rotYBeforeSpeed;   //Y軸回転の速さ

    [SerializeField] private float climbSpeed;      //梯子上る速さ

    void Start()
    {
        playerTrans = playerCC.transform;
        cmaTrans = playerCamera.transform;

        touchLadderFlg = false;
        closeLadderFlg = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "ladder") return;
        closeLadderFlg = true;

        //梯子の情報を入力
        ladderStartPos = other.gameObject.transform.GetChild(2).gameObject.transform.position;
        ladderStartRot = other.gameObject.transform.GetChild(2).gameObject.transform.eulerAngles;
        ladderStartQua = other.gameObject.transform.GetChild(2).gameObject.transform.rotation;
        ladderEndPos = other.gameObject.transform.GetChild(3).gameObject.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        closeLadderFlg = false;
    }

    void Update()
    {
        if (closeLadderFlg && Input.GetKeyDown(KeyCode.Space))
        {
            touchLadderFlg = true;
        }

        if (!touchLadderFlg) return;

        if (!MoveBeforeFinishFlg())
        {
            MoveLadderBefore();
        }

        if (MoveBeforeFinishFlg())
        {
            ClimbLadder();

            DescendLadder();

            GoUpLadder();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            FinishLadder();
        }
    }

    private void MoveLadderBefore() //梯子の前までセッティング
    {
        MoveLadderBeforePosition();

        MoveLadderBeforeRotation();
    }

    private void MoveLadderBeforePosition() //梯子の前まで移動
    {
        //移動する方向を計算
        Vector3 moveDir = ladderStartPos - playerTrans.position;
        moveDir.y = 0;

        bool moveSmallFlg = Mathf.Abs(moveDir.magnitude) <= moveBeforeSpeed * Time.deltaTime;   //移動量が小さすぎるか
        if (moveSmallFlg)
        {
            moveBeforeFlg = true;
        }
        else
        {
            //移動量を一定にする
            moveDir.Normalize();
            moveDir = moveDir * moveBeforeSpeed * Time.deltaTime;
        }
        playerCC.Move(moveDir);
    }

    private void MoveLadderBeforeRotation() //梯子の方に向く
    {
        RotationX();

        RotationY();
    }

    private void RotationX()    //X軸回転
    {
        //プレイヤーの向きを算出
        float camLocalRotX = cmaTrans.rotation.eulerAngles.x;

        if (camLocalRotX >= 180.0f)
        {
            camLocalRotX -= 360.0f;
        }

        //プレイヤーの梯子から見た角度を算出
        float playerRotX = camLocalRotX - ladderStartRot.x;

        bool rotLargeFlg = Mathf.Abs(playerRotX) > rotXBeforeSpeed * Time.deltaTime;    //角度が大きすぎるか
        if (rotLargeFlg)
        {
            //一定量回転する
            if (camLocalRotX > ladderStartRot.x)
            {
                cmaTrans.localRotation *= Quaternion.Euler(new Vector3(-rotXBeforeSpeed * Time.deltaTime, 0f, 0f));
            }
            else
            {
                cmaTrans.localRotation *= Quaternion.Euler(new Vector3(rotXBeforeSpeed * Time.deltaTime, 0f, 0f));
            }
        }
        else
        {
            //ぴったり回転する
            rotXBeforeFlg = true;
            cmaTrans.localRotation = Quaternion.Euler(new Vector3(ladderStartRot.x, 0f, 0f));
        }
    }

    private void RotationY()    //Y軸回転
    {
        //プレイヤーの向きを算出
        float playerLocalRotY = playerTrans.rotation.eulerAngles.y;

        if (playerLocalRotY >= 180.0f)
        {
            playerLocalRotY -= 360.0f;
        }

        //プレイヤーの梯子から見た角度を算出
        float ladderRotY = ladderStartRot.y - 180.0f;
        float playerRotY = playerLocalRotY - (ladderStartRot.y - 180.0f);

        bool rotLargeFlg = Mathf.Abs(playerRotY) > rotYBeforeSpeed * Time.deltaTime;    //角度が大きすぎるか
        if (rotLargeFlg)
        {
            //一定量回転する
            float tmp = playerTrans.rotation.eulerAngles.y - ladderStartRot.y;
            if (tmp < 0)
            {
                tmp += 360.0f;
            }
            if (tmp > 180.0f)
            {
                playerTrans.localRotation *= Quaternion.Euler(new Vector3(0f, -rotYBeforeSpeed * Time.deltaTime, 0f));
            }
            else
            {
                playerTrans.localRotation *= Quaternion.Euler(new Vector3(0f, rotYBeforeSpeed * Time.deltaTime, 0f));
            }
        }
        else
        {
            //ぴったり回転する
            rotYBeforeFlg = true;
            playerTrans.localRotation = Quaternion.Euler(new Vector3(0f, ladderStartRot.y - 180.0f, 0f));
        }
    }

    private bool MoveBeforeFinishFlg()  //梯子のセッティング終了
    {
        if (!moveBeforeFlg) return false;
        if (!rotXBeforeFlg) return false;
        if (!rotYBeforeFlg) return false;
        return true;
    }

    private void FinishLadder() //梯子上り終える
    {
        closeLadderFlg = false;
        touchLadderFlg = false;
        moveBeforeFlg = false;
        rotXBeforeFlg = false;
        rotYBeforeFlg = false;
    }

    private void ClimbLadder()  //梯子を上り下りする
    {
        if (playerTrans.position.y > ladderEndPos.y) return;

        //梯子の向きを算出
        Vector3 climbVec = Vector3.zero;
        Quaternion ladderQuaX = Quaternion.Euler(-ladderStartRot.x, ladderStartRot.y, 0);

        if (Input.GetKey(KeyCode.W))    //上る処理
        {
            climbVec = ladderQuaX * new Vector3(0, climbSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.S))    //下がる処理
        {
            climbVec = ladderQuaX * new Vector3(0, -climbSpeed, 0);
        }

        playerCC.Move(climbVec * Time.deltaTime);
    }

    private void DescendLadder()    //梯子を降りる処理
    {
        if (!playerCC.isGrounded) return;

        if (Input.GetKey(KeyCode.S))
        {
            FinishLadder();
        }
    }

    private void GoUpLadder()   //梯子を登りきる処理
    {
        //一番上まで登ってなかったら、処理をしない
        if (playerTrans.position.y < ladderEndPos.y) return;

        //梯子の向きを算出
        Vector3 climbVec = Vector3.zero;
        Quaternion ladderQuaX = Quaternion.Euler(0, ladderStartRot.y, 0);

        //登りきる処理
        if (Input.GetKey(KeyCode.W))
        {
            climbVec = ladderQuaX * new Vector3(0, 0, climbSpeed);  //まっすぐ進む
        }
        playerCC.Move(-climbVec * Time.deltaTime);

        //プレイヤーの下にレイを飛ばし、登り切ったから判定する
        int layerMask = 1 << 13;
        Ray ray = new Ray(playerTrans.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (!(hit.collider.gameObject.tag == "wall")) return;
            FinishLadder();
        }
    }

    public bool ClimbLadderFlg()    //梯子を登っているか
    {
        if (!closeLadderFlg) return false;
        if (!touchLadderFlg) return false;
        if (!MoveBeforeFinishFlg()) return false;
        return true;
    }
}