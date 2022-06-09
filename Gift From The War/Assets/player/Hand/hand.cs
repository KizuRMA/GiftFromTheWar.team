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
        this.GetComponent<Image>().enabled = false; //最初は表示しない
    }

    void Update()
    {
        if(ladder.closeLadderFlg || metalDoor.closeValveFlg)  //梯子の近くなら表示
        {
            this.GetComponent<Image>().enabled = true;
        }
        else
        {
            this.GetComponent<Image>().enabled = false;
        }

        if (ladder.touchLadderFlg || metalDoor.touchValveFlg)  //梯子に登り始めたら非表示
        {
            this.GetComponent<Image>().enabled = false;
        }
    }
}
