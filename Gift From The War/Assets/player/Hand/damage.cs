using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class damage : MonoBehaviour
{
    [SerializeField] playerAbnormalcondition abnormalcondition;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (abnormalcondition.life <= 1)
        {
            this.GetComponent<Image>().enabled = true;
        }
    }
}
