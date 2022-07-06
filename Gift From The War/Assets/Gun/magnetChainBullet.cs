using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnetChainBullet : MonoBehaviour
{
    private magnetChain magChain;

    void Start()
    {
        magChain = GameObject.Find("muzzlePos").GetComponent<magnetChain>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "dogAgent" || other.gameObject.tag == "Detector") return; //ìñÇΩÇ¡ÇΩÇÃÇ™dogAgentÇæÇ¡ÇΩÇÁèàóùÇµÇ»Ç¢

        if (other.gameObject.tag == "metal" || other.gameObject.tag == "fixedMetal" || other.gameObject.tag == "Dog1" || other.gameObject.tag == "valve" || other.gameObject.tag == "gimmickButton")
        {
            magChain.metalFlg = true;
        }
        Destroy(this.gameObject);
    }
}
