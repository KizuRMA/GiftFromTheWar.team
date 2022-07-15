using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjSE : MonoBehaviour
{
    [SerializeField] private string SEName;
    [SerializeField] private float minVol;  //�{�����[���̍ŏ��l
    [SerializeField] private float velocityVolRaito;  //�����ɂ��{�����[���̕␳�{��
    [SerializeField] private float volDis;

    private float pastPosY; //�I�u�W�F�N�g�̑O�̍��W�ۑ����Ă���
    private bool moveFlg;   //�I�u�W�F�N�g����������

    void Start()
    {
    }

    void Update()
    {
        moveFlg = (pastPosY != this.transform.position.y);
        pastPosY = this.transform.position.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!moveFlg) return;
        if (collision.transform.tag == "Detector") return;

        moveFlg = false;
        float minHeight = this.transform.position.y;

        //�␳�v�Z
        float velocityVol = this.GetComponent<Rigidbody>().velocity.magnitude * velocityVolRaito;

        AudioManager.Instance.PlaySE(SEName, this.gameObject, volDis, isLoop: false, vol: minVol + velocityVol);
    }
}
