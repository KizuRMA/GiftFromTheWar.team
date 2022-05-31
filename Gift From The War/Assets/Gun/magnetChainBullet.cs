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
        if (other.gameObject.tag == "dogAgent") return; //“–‚½‚Á‚½‚Ì‚ªdogAgent‚¾‚Á‚½‚çˆ—‚µ‚È‚¢

        if (other.gameObject.tag == "metal" || other.gameObject.tag == "Dog1")
        {
            magChain.metalFlg = true;
        }
        Destroy(this.gameObject);
    }
}
