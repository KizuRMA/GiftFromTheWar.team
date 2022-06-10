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
        if (other.gameObject.tag == "dogAgent") return; //���������̂�dogAgent�������珈�����Ȃ�

        if (other.gameObject.tag == "metal" || other.gameObject.tag == "Dog1")
        {
            mag.metal = other.gameObject;
            if (other.gameObject.tag == "Dog1")
            {
                var state = other.GetComponent<DogState>();
                state.MagnetCatch();
            }
        }
        Destroy(this.gameObject);
    }
}
