using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGimmick : MonoBehaviour
{
    [SerializeField] private HandButton button1;
    [SerializeField] private HandButton button2;
    [SerializeField] private HandButton button3;
    [SerializeField] private GameObject light1;
    [SerializeField] private GameObject light2;
    [SerializeField] private GameObject light3;
    [SerializeField] private Material mat;

    //ドアを開ける
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private GameObject leftDoor;
    private bool openFlg = false;

    [SerializeField] private float movePower;
    [SerializeField] private float maxPos;
    private float nowPower;
    private float sumPos;

    private int changeCount;

    void Start()
    {
        
    }

    void Update()
    {
        //何個ボタンが押されているか
        changeCount = 0;
        if (button1.changeFlg) changeCount++;
        if (button2.changeFlg) changeCount++;
        if (button3.changeFlg) changeCount++;

        //ライトを光らせる処理
        if(changeCount >= 1)
        {
            light1.GetComponent<MeshRenderer>().material = mat;
        }

        if(changeCount >= 2)
        {
            light2.GetComponent<MeshRenderer>().material = mat;
        }

        if (changeCount >= 3)
        {
            light3.GetComponent<MeshRenderer>().material = mat;
            openFlg = true;
        }

        //ドアを開ける
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
    }

    private void CloseDoor()
    {
        if (sumPos <= 0) return;
        nowPower = -movePower * Time.deltaTime;
    }
}
