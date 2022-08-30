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
    [SerializeField] private Material clearMat;

    //ドアを開ける
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private GameObject leftDoor;
    [System.NonSerialized]public bool openFlg = false;

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
        //何個ボタンが押されているか
        changeCount = 0;
        if (button1 != null && button1.changeFlg) changeCount++;
        if (button2 != null && button2.changeFlg) changeCount++;
        if (button3 != null && button3.changeFlg) changeCount++;

        switch (changeCount)
        {
            case 0:
                light1.GetComponent<MeshRenderer>().material = offMat;
                light2.GetComponent<MeshRenderer>().material = offMat;
                light3.GetComponent<MeshRenderer>().material = offMat;
                break;
            case 1:
                light1.GetComponent<MeshRenderer>().material = onMat;
                light2.GetComponent<MeshRenderer>().material = offMat;
                light3.GetComponent<MeshRenderer>().material = offMat;
                break;
            case 2:
                light1.GetComponent<MeshRenderer>().material = onMat;
                light2.GetComponent<MeshRenderer>().material = onMat;
                light3.GetComponent<MeshRenderer>().material = offMat;
                break;
            case 3:
                light1.GetComponent<MeshRenderer>().material = onMat;
                light2.GetComponent<MeshRenderer>().material = onMat;
                light3.GetComponent<MeshRenderer>().material = onMat;
                Open();
                break;

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

    public void HandButtonDelete(ref HandButton _useButton)
    {
        if (_useButton != null)
        {
            _useButton.gimmick = null;
            _useButton = null;
        }
    }

    public void Open()
    {
        openFlg = true;

        light1.GetComponent<MeshRenderer>().material = clearMat;
        light2.GetComponent<MeshRenderer>().material = clearMat;
        light3.GetComponent<MeshRenderer>().material = clearMat;
    }
}
