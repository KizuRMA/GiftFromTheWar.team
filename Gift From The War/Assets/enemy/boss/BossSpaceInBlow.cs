using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpaceInBlow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody _rd = transform.GetComponent<Rigidbody>();
        BoxCollider _box = transform.GetComponent<BoxCollider>();

        if (_rd == null || _box == null) return;


    }
}
