using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnetBullet : MonoBehaviour
{
    private magnet mag;

    void Start()
    {
        mag = GameObject.Find("muzzlePos").GetComponent<magnet>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "dogAgent") return; //当たったのがdogAgentだったら処理しない

        if (other.gameObject.tag == "metal" || other.gameObject.tag == "Dog1")
        {
            mag.metal = other.gameObject;
        }
        Destroy(this.gameObject);
    }
}
