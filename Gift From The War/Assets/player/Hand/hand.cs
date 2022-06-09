using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hand : MonoBehaviour
{
    [SerializeField] private playerHundLadder ladder;
    [SerializeField] private OpenMetalDoor metalDoor;

    void Start()
    {
        this.GetComponent<Image>().enabled = false; //�ŏ��͕\�����Ȃ�
    }

    void Update()
    {
        if(ladder.closeLadderFlg || metalDoor.closeValveFlg)  //��q�̋߂��Ȃ�\��
        {
            this.GetComponent<Image>().enabled = true;
        }
        else
        {
            this.GetComponent<Image>().enabled = false;
        }

        if (ladder.touchLadderFlg || metalDoor.touchValveFlg)  //��q�ɓo��n�߂����\��
        {
            this.GetComponent<Image>().enabled = false;
        }
    }
}
