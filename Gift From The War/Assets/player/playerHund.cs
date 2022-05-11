using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHundLadder : MonoBehaviour
{
    [SerializeField] CharacterController playerCC;
    [SerializeField] GameObject playerCamera;

    Vector3 ladderPos;
    float ladderRotY;
    Vector3 playerMoveVec;

    bool touchLadderFlg = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(touchLadderFlg);

        MoveLadderBefore();

        ClimbLadder();

        playerCC.Move(playerMoveVec);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ladder")
        {
            touchLadderFlg = true;
            ladderPos = other.gameObject.transform.position;
            ladderRotY = other.gameObject.transform.rotation.y;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ladder")
        {
            touchLadderFlg = false;
        }
    }

    private void MoveLadderBefore()
    {

    }

    private void ClimbLadder()
    {

    }
}
