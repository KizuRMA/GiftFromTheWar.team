using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBigCollison : MonoBehaviour
{
    public bool hitFlg { get; set; }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "dogAgent" || other.gameObject.tag == "Detector") return;
        hitFlg = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "dogAgent" || other.gameObject.tag == "Detector") return;
        hitFlg = false;
    }
}
