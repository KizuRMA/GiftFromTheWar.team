using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMetalDoor : MonoBehaviour
{
    //オブジェクトやスクリプトをとってくる
    [SerializeField] private Transform camTrans;

    //レイ判定
    [SerializeField] private float valveDis;    //バルブに手が届く範囲
    public bool closeValveFlg { get; set; }     //バルブの近くにいるか
    private valve valve;                        //バルブの情報

    //回す処理
    public bool touchValveFlg { get; set; }     //バルブを触っているか

    void Start()
    {
        closeValveFlg = false;
    }

    void Update()
    {
        ValveRay();

        if (!closeValveFlg) return; //バルブの近くにいないなら処理しない

        OpenDoor();
    }

    private void ValveRay()
    {
        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, valveDis))
        {
            if (hit.collider.tag != "valve")
            {
                closeValveFlg = false;
                touchValveFlg = false;
                return;
            }

            if (closeValveFlg) return; //すでに情報を入力していたら処理しない

            //バルブの情報を入力
            valve = hit.collider.GetComponent<valve>();

            closeValveFlg = true;
        }
        else
        {
            closeValveFlg = false;
            touchValveFlg = false;
        }
    }

    private void OpenDoor()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            touchValveFlg = true;
            valve.Open();
        }
        else
        {
            touchValveFlg = false;
        }
    }
}
