using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGimmick : MonoBehaviour
{
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private GameObject leftDoor;
    private bool openFlg;

    [SerializeField] private float movePower;
    [SerializeField] private float maxPos;
    private float nowPower;
    private float sumPos;
    private bool switchFlg = false;

    void Start()
    {
        
    }

    void Update()
    {
        nowPower = 0;

        if (openFlg)
        {
            OpneDoor();
        }
        else
        {
            CloseDoor();
        }

        sumPos += nowPower;
        rightDoor.transform.localPosition += new Vector3(nowPower, 0, 0);
        leftDoor.transform.localPosition += new Vector3(-nowPower, 0, 0);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "GimmickBall") return;
        openFlg = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "GimmickBall") return;
        openFlg = false;
    }

    private void OpneDoor()
    {
        if (sumPos >= maxPos) return;
        nowPower = movePower * Time.deltaTime;

        if(!switchFlg)
        {
            AudioManager.Instance.PlaySE("BallPlate", isLoop: false);
            switchFlg = true;
        }

        AudioManager.Instance.PlaySE("OpenDoor", isLoop: false);
    }

    private void CloseDoor()
    {
        if (sumPos <= 0) return;
        nowPower = -movePower * Time.deltaTime;
        AudioManager.Instance.PlaySE("OpenDoor", isLoop: false);
        switchFlg = false;
    }
}
