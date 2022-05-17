using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rantanLadder : MonoBehaviour
{
    [SerializeField] CharacterController playerCC;
    [SerializeField] playerHundLadder playerHund;
    private Transform trans;
    private Vector3 firstPos;
    [SerializeField] private float upDownSpeed;
    [SerializeField] private float maxPosY;

    bool finishFlg = true;

    // Start is called before the first frame update
    void Start()
    {
        trans = transform;
        firstPos = trans.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHund.ClimbLadderFlg())
        {
            TouchLadder();
            return;
        }

        if (finishFlg) return;
        NoTouchLadder();
    }

    private void TouchLadder()
    {
        if (trans.localPosition.y < firstPos.y - maxPosY)
        {
            finishFlg = false;
            return;
        }

        trans.localPosition += new Vector3(0, -upDownSpeed, 0);
    }

    private void NoTouchLadder()
    {
        if (trans.localPosition.y > firstPos.y)
        {
            finishFlg = true;
            return;
        }

        trans.localPosition += new Vector3(0, upDownSpeed, 0);
    }
}
