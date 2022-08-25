using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGimmick : MonoBehaviour
{
    [SerializeField] public HandButton button1;
    [SerializeField] public HandButton button2;
    [SerializeField] public HandButton button3;
    [SerializeField] private GameObject light1;
    [SerializeField] private GameObject light2;
    [SerializeField] private GameObject light3;
    [SerializeField] private Material onMat;
    [SerializeField] private Material offMat;

    //ドアを開ける
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private GameObject leftDoor;
    private bool openFlg = false;

    [SerializeField] private float movePower;
    [SerializeField] private float maxPos;
    private float nowPower;
    private float sumPos;

    private int changeCount;

    private void Awake()
    {
        if (button1 != null)
        {
            button1.gimmick = this;
        }

        if (button2 != null)
        {
            button2.gimmick = this;
        }

        if (button3 != null)
        {
            button3.gimmick = this;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (button1 == null || button2 == null || button3 == null)
        {
            if (button1 == null)
            {
                light1.GetComponent<MeshRenderer>().material = offMat;
            }

            if (button2 == null)
            {
                light2.GetComponent<MeshRenderer>().material = offMat;
            }

            if (button3 == null)
            {
                light3.GetComponent<MeshRenderer>().material = offMat;
            }
            return;
        }

        //何個ボタンが押されているか
        changeCount = 0;
        if (button1.changeFlg) changeCount++;
        if (button2.changeFlg) changeCount++;
        if (button3.changeFlg) changeCount++;

        //ライトを光らせる処理
        if(changeCount >= 1)
        {
            light1.GetComponent<MeshRenderer>().material = onMat;
        }

        if(changeCount >= 2)
        {
            light2.GetComponent<MeshRenderer>().material = onMat;
        }

        if (changeCount >= 3)
        {
            light3.GetComponent<MeshRenderer>().material = onMat;
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

    public void HandButtonChange(ref HandButton _useButton,ref HandButton _putInButton)
    {
        if (_useButton == null)
        {
            _useButton = _putInButton;
            _useButton.gimmick = this;
        }
        else
        {
            _useButton.gimmick = null;

            _useButton = _putInButton;
            _useButton.gimmick = this;
        }
    }
}
