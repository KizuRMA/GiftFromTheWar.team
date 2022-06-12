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
        Debug.Log(other.gameObject);
        if (other.gameObject.tag == "dogAgent" || other.gameObject.tag == "Detector") return; //当たったのがdogAgentだったら処理しない

        if (other.gameObject.tag == "metal" || other.gameObject.tag == "Dog1" || other.gameObject.tag == "Bat")
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
