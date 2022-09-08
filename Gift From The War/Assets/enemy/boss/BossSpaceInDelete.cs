using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpaceInDelete : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Boss") return;

        transform.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Boss") return;

        transform.gameObject.SetActive(false);
    }


}
