using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hand : MonoBehaviour
{
    [SerializeField] private playerHundLadder ladder;
    [SerializeField] private OpenMetalDoor metalDoor;
    [SerializeField] private GetItem item;

    [SerializeField] private Image image;

    void Start()
    {
        image = this.GetComponent<Image>();
        image.enabled = false; //ç≈èâÇÕï\é¶ÇµÇ»Ç¢
    }

    void Update()
    {
        if(ladder.closeLadderFlg || metalDoor.closeValveFlg || item.closeItemFlg)
        {
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
        }

        if (ladder.touchLadderFlg || metalDoor.touchValveFlg)
        {
            image.enabled = false;
        }
    }
}
