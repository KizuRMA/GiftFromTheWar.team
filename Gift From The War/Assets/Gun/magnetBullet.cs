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
        if (other.gameObject.tag == "dogAgent" || other.gameObject.tag == "Detector" || other.gameObject.tag == "Untagged") return; //ìñÇΩÇ¡ÇΩÇÃÇ™dogAgentÇæÇ¡ÇΩÇÁèàóùÇµÇ»Ç¢
        if (mag.metal != null) return;

        if (other.gameObject.tag == "metal" || other.gameObject.tag == "Dog1" || other.gameObject.tag == "Bat" || other.gameObject.tag == "gimmickButton")
        {
            mag.metal = other.gameObject;

            var enemyInter = other.GetComponent<EnemyInterface>();

            if (enemyInter != null)
            {
                enemyInter.MagnetCatch();
            }
        }
        Destroy(this.gameObject);
    }
}
