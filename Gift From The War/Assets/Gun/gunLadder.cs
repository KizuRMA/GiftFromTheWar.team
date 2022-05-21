using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunLadder : MonoBehaviour
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

    void Update()
    {
        if (playerHund.ClimbLadderFlg())
        {
            TouchLadder();
            return;
        }

        NoTouchLadder();
    }

    private void TouchLadder()
    {
        if (!finishFlg) return;

        if (trans.localPosition.y < firstPos.y - maxPosY)
        {
            finishFlg = false;
            return;
        }

        trans.localPosition += new Vector3(0, -upDownSpeed, 0) * Time.deltaTime;
    }

    private void NoTouchLadder()
    {
        if (finishFlg) return;

        if (trans.localPosition.y > firstPos.y)
        {
            finishFlg = true;
            return;
        }

        trans.localPosition += new Vector3(0, upDownSpeed, 0) * Time.deltaTime;
    }
}
