using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hand : MonoBehaviour
{
    [SerializeField] playerHundLadder ladder;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(ladder.closeLadderFlg)
        {
            this.GetComponent<Image>().enabled = true;
        }
        else
        {
            this.GetComponent<Image>().enabled = false;
        }

        if (ladder.touchLadderFlg)
        {
            this.GetComponent<Image>().enabled = false;
        }
    }
}
