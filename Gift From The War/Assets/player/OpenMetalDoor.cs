using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMetalDoor : MonoBehaviour
{
    //�I�u�W�F�N�g��X�N���v�g���Ƃ��Ă���
    [SerializeField] private Transform camTrans;

    //���C����
    [SerializeField] private float valveDis;    //�o���u�Ɏ肪�͂��͈�
    public bool closeValveFlg { get; set; }     //�o���u�̋߂��ɂ��邩
    private valve valve;                        //�o���u�̏��

    //�񂷏���
    public bool touchValveFlg { get; set; }     //�o���u��G���Ă��邩

    void Start()
    {
        closeValveFlg = false;
    }

    void Update()
    {
        ValveRay();

        if (!closeValveFlg) return; //�o���u�̋߂��ɂ��Ȃ��Ȃ珈�����Ȃ�

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

            if (closeValveFlg) return; //���łɏ�����͂��Ă����珈�����Ȃ�

            //�o���u�̏������
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
