using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class valve : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g
    [SerializeField] private Transform valceTrans;
    [SerializeField] private Transform door1;
    [SerializeField] private Transform door2;

    //�񂷏���
    [SerializeField] private float valveSpeed;
    [SerializeField] private float door1Speed;
    [SerializeField] private float door2Speed;
    [SerializeField] private float openAngMax;
    private float sumAng = 0;

    void Start()
    {
    }

    void Update()
    {
    }

    public void Open()
    {
        if (sumAng > Mathf.Abs(openAngMax)) return;  //�ő�܂ŊJ���Ă����珈�����Ȃ�
        valceTrans.localEulerAngles += new Vector3(0, valveSpeed * Time.deltaTime, 0);
        door1.localEulerAngles += new Vector3(0, door1Speed * Time.deltaTime, 0);
        door2.localEulerAngles += new Vector3(0, door2Speed * Time.deltaTime, 0);
        sumAng += Mathf.Abs(door1Speed * Time.deltaTime);
        AudioManager.Instance.PlaySE("OpenDoor2", isLoop: false);

    }
}
