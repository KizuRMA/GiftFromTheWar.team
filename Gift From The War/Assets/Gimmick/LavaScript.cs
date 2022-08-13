using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Dog1") return;

        var target = other.transform.GetComponent<EnemyInterface>();
        target.LavaDamage();
    }
}
