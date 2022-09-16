using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGimmick : MonoBehaviour
{
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject playerKeyObj;
    [SerializeField] private GameObject keyObj;
    [SerializeField] private GameObject gunObj;

    private bool openFlg;

    [SerializeField] private float movePower;
    [SerializeField] private float maxPos;
    private float nowPower;
    private float sumPos;
    private bool switchFlg = false;

    private void Awake()
    {

    }
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

    private void OpneDoor()
    {
        if (sumPos >= maxPos) return;
        nowPower = movePower * Time.deltaTime;

        //âπÇçƒê∂
        if (!switchFlg)
        {
            AudioManager.Instance.PlaySE("BallPlate", gameObject, isLoop: false);
            switchFlg = true;
        }

        AudioManager.Instance.PlaySE("OpenDoor1", gameObject, isLoop: false);

    }

    private void CloseDoor()
    {
        if (sumPos <= 0) return;
        nowPower = -movePower * Time.deltaTime;

        //âπÇçƒê∂
        AudioManager.Instance.PlaySE("OpenDoor1", gameObject, isLoop: false);
        switchFlg = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerKeyObj == null || keyObj == null || other.gameObject.tag != "Player") return;

        if (playerKeyObj.activeSelf == true)
        {
            openFlg = true;
            playerKeyObj.SetActive(false);
            keyObj.SetActive(true);
            gunObj.SetActive(true);
        }
    }
}
