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
        if (other.gameObject.tag == "Detector") return; //���������̂�"Detector"�^�O�̏ꍇ�͏������Ȃ�

        Destroy(this.gameObject);
    }
}
