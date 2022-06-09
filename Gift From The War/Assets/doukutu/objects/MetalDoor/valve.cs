using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class valve : MonoBehaviour
{
    //ゲームオブジェクト
    [SerializeField] private Transform valceTrans;
    [SerializeField] private Transform door1;
    [SerializeField] private Transform door2;

    //回す処理
    [SerializeField] private float valveSpeed;
    [SerializeField] private float door1Speed;
    [SerializeField] private float door2Speed;

    void Start()
    {
    }

    void Update()
    {      
    }

    public void Open()
    {
        valceTrans.localEulerAngles += new Vector3(0, valveSpeed * Time.deltaTime, 0);
        door1.localEulerAngles += new Vector3(0, door1Speed * Time.deltaTime, 0);
        door2.localEulerAngles += new Vector3(0, door2Speed * Time.deltaTime, 0);
    }
}
