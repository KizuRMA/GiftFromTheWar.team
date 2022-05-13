using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHundLadder : MonoBehaviour
{
    [SerializeField] CharacterController playerCC;
    [SerializeField] GameObject playerCamera;
    Transform playerTrans;
    Transform cmaTrans;

    Vector3 ladderPos;
    Vector3 ladderRot;
    Vector3 playerMoveVec;

    [SerializeField] float warpSpeed;
    [SerializeField] float warpPosMin;
    [SerializeField] float warpRotSpeed;

    bool touchLadderFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = playerCC.transform;
        cmaTrans = playerCamera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        MoveLadderBefore();

        ClimbLadder();

        if(Input.GetKeyDown(KeyCode.E))
        {
            touchLadderFlg = false;
        }
        //Debug.Log(cmaTrans.localRotation.eulerAngles.x);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ladder")
        {
            touchLadderFlg = true;
            ladderPos = other.gameObject.transform.GetChild(2).gameObject.transform.position;
            ladderRot = other.gameObject.transform.GetChild(2).gameObject.transform.eulerAngles;
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
        if (touchLadderFlg)
        {
            MoveLadderBeforePosition();

            MoveLadderBeforeRotation();
        }
    }

    private void MoveLadderBeforePosition()
    {
        Vector3 warpDir = ladderPos - transform.position;
        warpDir.y = 0;
        if (warpDir.magnitude > warpPosMin)
        {
            warpDir.Normalize();
            warpDir = warpDir * warpSpeed;
        }

        playerCC.Move(warpDir);
    }

    private void MoveLadderBeforeRotation()
    {
        float playerLocalRotY = playerTrans.localRotation.eulerAngles.y;

        if(playerLocalRotY >= 180.0f)
        {
            playerLocalRotY -= 360.0f;
        }

        float playerRotY = playerLocalRotY - (ladderRot.y - 180.0f);
        if (Mathf.Abs(playerRotY) > warpRotSpeed)
        {
            if(playerLocalRotY > ladderRot.y - 180.0f)
            {
                playerTrans.localRotation *= Quaternion.Euler(new Vector3(0f, -warpRotSpeed, 0f));
            }
            else
            {
                playerTrans.localRotation *= Quaternion.Euler(new Vector3(0f, warpRotSpeed, 0f));
            }
        }
        else
        {
            playerTrans.localRotation = Quaternion.Euler(new Vector3(0f, ladderRot.y - 180.0f, 0f));
        }

        float camLocalRotX = cmaTrans.localRotation.eulerAngles.x;

        if (camLocalRotX >= 180.0f)
        {
            camLocalRotX -= 360.0f;
        }

        float playerRotX = camLocalRotX - ladderRot.x;
        if (Mathf.Abs(playerRotX) > warpRotSpeed)
        {
            if (camLocalRotX > ladderRot.x)
            {
                cmaTrans.localRotation *= Quaternion.Euler(new Vector3(-warpRotSpeed, 0f, 0f));
            }
            else
            {
                cmaTrans.localRotation *= Quaternion.Euler(new Vector3(warpRotSpeed, 0f, 0f));
            }
        }
        else
        {
            cmaTrans.localRotation = Quaternion.Euler(new Vector3(ladderRot.x, 0f, 0f));
        }

        //playerTrans.localRotation = Quaternion.Euler(new Vector3(0f, ladderRot.y - 180.0f, 0f));
        //cmaTrans.localRotation = Quaternion.Euler(new Vector3(ladderRot.x, 0f, 0f));
    }

    private void ClimbLadder()
    {

    }


    public bool GetTouchLadderFlg()
    {
        return touchLadderFlg;
    }
}
