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
        if (other.gameObject.tag == "dogAgent") return; //ìñÇΩÇ¡ÇΩÇÃÇ™dogAgentÇæÇ¡ÇΩÇÁèàóùÇµÇ»Ç¢

        if (other.gameObject.tag == "metal" || other.gameObject.tag == "Dog1" || other.gameObject.tag == "Bat")
        {
            mag.metal = other.gameObject;
            if (other.gameObject.tag == "Dog1")
            {
                var state = other.GetComponent<DogState>();
                state.MagnetCatch();
            }

            if (other.gameObject.tag == "Bat")
            {
                var state = other.GetComponent<BatPatrolState>();
                state.MagnetCatch();
            }
        }
        Destroy(this.gameObject);
    }
}
