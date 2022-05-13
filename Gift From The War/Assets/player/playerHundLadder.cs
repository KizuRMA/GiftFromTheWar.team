using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHundLadder : MonoBehaviour
{
    [SerializeField] CharacterController playerCC;
    [SerializeField] GameObject playerCamera;
    Transform playerTrans;
    Transform cmaTrans;

    bool touchLadderFlg = false;

    Vector3 ladderPos;
    Vector3 ladderRot;
    Vector3 ladderEndPos;
    Quaternion ladderQua;

    bool moveBeforeFlg = false;
    bool rotXBeforeFlg = false;
    bool rotYBeforeFlg = false;

    [SerializeField] float warpSpeed;
    [SerializeField] float warpRotSpeed;
    [SerializeField] float warpRotXSpeed;

    [SerializeField] float climbSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = playerCC.transform;
        cmaTrans = playerCamera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float playerLocalRotY = playerTrans.rotation.eulerAngles.y;

        if (playerLocalRotY >= 180.0f)
        {
            playerLocalRotY -= 360.0f;
        }
        Debug.Log(playerLocalRotY);

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ladder")
        {
            touchLadderFlg = true;
            ladderPos = other.gameObject.transform.GetChild(2).gameObject.transform.position;
            ladderRot = other.gameObject.transform.GetChild(2).gameObject.transform.eulerAngles;
            ladderEndPos = other.gameObject.transform.GetChild(3).gameObject.transform.position;
            ladderQua = other.gameObject.transform.GetChild(2).gameObject.transform.rotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ladder")
        {
            //touchLadderFlg = false;
        }
    }

    private void MoveLadderBefore()
    {
        MoveLadderBeforePosition();

        MoveLadderBeforeRotation();
    }

    private void MoveLadderBeforePosition()
    {
        Vector3 warpDir = ladderPos - playerTrans.position;
        warpDir.y = 0;
        warpDir.z = 0;
        if (warpDir.magnitude > warpSpeed)
        {
            warpDir.Normalize();
            warpDir = warpDir * warpSpeed;
        }
        else
        {
            moveBeforeFlg = true;
        }

        playerCC.Move(warpDir);
    }

    private void MoveLadderBeforeRotation()
    {
        float playerLocalRotY = playerTrans.rotation.eulerAngles.y;

        if (playerLocalRotY >= 180.0f)
        {
            playerLocalRotY -= 360.0f;
        }

        float playerRotY = playerLocalRotY - (ladderRot.y - 180.0f);
        //if (Mathf.Abs(playerRotY) > warpRotSpeed)
        //{
        //    //if (playerLocalRotY > ladderRot.y)
        //    //{
        //    //    playerTrans.localRotation *= Quaternion.Euler(new Vector3(0f, -warpRotSpeed, 0f));
        //    //}
        //    //else
        //    //{
        //    //    playerTrans.localRotation *= Quaternion.Euler(new Vector3(0f, warpRotSpeed, 0f));
        //    //}
        //    Debug.Log(ladderRot.y);

        //    playerTrans.localRotation = Quaternion.RotateTowards(playerTrans.rotation, ladderQua, warpRotSpeed);
        //}
        //else
        //{
        //    rotXBeforeFlg = true;
        //    playerTrans.localRotation = Quaternion.Euler(new Vector3(0f, ladderRot.y - 180.0f, 0f));
        //}

        Quaternion ladderQuaY = ladderQua;
        ladderQuaY[0] = 0;
        ladderQuaY[2] = 0;
        playerTrans.localRotation = Quaternion.RotateTowards(playerTrans.rotation, ladderQuaY, warpRotSpeed);
        if (Mathf.Abs(playerRotY) <= warpRotSpeed)
        {
            rotYBeforeFlg = true;
        }

        float camLocalRotX = cmaTrans.rotation.eulerAngles.x;

        if (camLocalRotX >= 180.0f)
        {
            camLocalRotX -= 360.0f;
        }

        float playerRotX = camLocalRotX - ladderRot.x;
        if (Mathf.Abs(playerRotX) > warpRotXSpeed)
        {
            if (camLocalRotX > ladderRot.x)
            {
                cmaTrans.localRotation *= Quaternion.Euler(new Vector3(-warpRotXSpeed, 0f, 0f));
            }
            else
            {
                cmaTrans.localRotation *= Quaternion.Euler(new Vector3(warpRotXSpeed, 0f, 0f));
            }
        }
        else
        {
            rotXBeforeFlg = true;
            cmaTrans.localRotation = Quaternion.Euler(new Vector3(ladderRot.x, 0f, 0f));
        }
    }

    private bool MoveBeforeFinishFlg()
    {
        if (!moveBeforeFlg) return false;
        if (!rotXBeforeFlg) return false;
        if (!rotYBeforeFlg) return false;
        return true;
    }

    private void FinishLadder()
    {
        touchLadderFlg = false;
        moveBeforeFlg = false;
        rotXBeforeFlg = false;
        rotYBeforeFlg = false;
    }

    private void ClimbLadder()
    {
        Vector3 climbVec = Vector3.zero;
        Quaternion ladderQuaX = Quaternion.Euler(ladderRot.x, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            climbVec = ladderQuaX * new Vector3(0, climbSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            climbVec = ladderQuaX * new Vector3(0, -climbSpeed, 0);
        }

        playerCC.Move(climbVec * Time.deltaTime);
    }

    private void DescendLadder()
    {
        if (!playerCC.isGrounded) return;

        if (Input.GetKey(KeyCode.S))
        {
            FinishLadder();
        }
    }

    private void GoUpLadder()
    {
        if (playerTrans.position.y < ladderEndPos.y) return;

        Vector3 climbVec = Vector3.zero;
        Quaternion ladderQuaX = Quaternion.Euler(0, ladderRot.y, 0);
        if (Input.GetKey(KeyCode.W))
        {
            climbVec = ladderQuaX * new Vector3(0, 0, climbSpeed);
        }
        playerCC.Move(-climbVec * Time.deltaTime);

        if (!playerCC.isGrounded) return;

        FinishLadder();
    }


    public bool GetTouchLadderFlg()
    {
        return touchLadderFlg;
    }
}
