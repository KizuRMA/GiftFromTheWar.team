using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpaceInBlow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Boss") return;

        Rigidbody _rd = transform.GetComponent<Rigidbody>();
        BoxCollider _box = transform.GetComponent<BoxCollider>();

        if (_rd == null || _box == null)
        {
            Destroy(gameObject);
        }

        var random = new System.Random();
        var min = 1;
        var max = 3;

        if (_rd != null && _box != null)
        {
            _rd.isKinematic = false;

            var vect = transform.position - other.transform.position;
            vect = vect.normalized * random.Next(min, max);
            _rd.AddForce(vect, ForceMode.Impulse);
            _rd.AddTorque(vect, ForceMode.Impulse);
        }

        Destroy(gameObject,2.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Boss") return;

        Rigidbody _rd = transform.GetComponent<Rigidbody>();
        BoxCollider _box = transform.GetComponent<BoxCollider>();

        if (_rd == null || _box == null)
        {
            Destroy(gameObject);
        }
        var random = new System.Random();
        var min = 1;
        var max = 3;

        if (_rd != null && _box != null)
        {
            _rd.isKinematic = false;

            var vect = transform.position - collision.transform.position;
            vect = vect.normalized * random.Next(min, max);
            _rd.AddForce(vect, ForceMode.Impulse);
            _rd.AddTorque(vect, ForceMode.Impulse);
        }

        Destroy(gameObject, 2.0f);
    }
}
