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
        if (other.gameObject.tag == "Detector") return; //“–‚½‚Á‚½‚Ì‚ª"Detector"ƒ^ƒO‚Ìê‡‚Íˆ—‚µ‚È‚¢

        Destroy(this.gameObject);
    }
}
