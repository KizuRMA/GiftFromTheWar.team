using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBullet : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "dogAgent" || other.gameObject.tag == "Detector" || other.gameObject.tag == "Untagged") return;

        Destroy(this.gameObject);
    }
}
