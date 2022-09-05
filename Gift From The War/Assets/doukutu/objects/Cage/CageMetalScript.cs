using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CageMetalScript : MonoBehaviour
{
    [SerializeField] NavMeshObstacle obstacle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Boss") return;
        if (obstacle == null || obstacle.enabled == true) return;

        var random = new System.Random();
        var min = 5;
        var max = 7;

        Rigidbody rd = GetComponent<Rigidbody>();
        BoxCollider collider = GetComponent<BoxCollider>();

        if (rd != null && collider != null)
        {
            rd.isKinematic = false;

            var vect = transform.position - other.transform.position;
            vect = vect.normalized * random.Next(min, max);
            rd.AddForce(vect, ForceMode.Impulse);
            rd.AddTorque(vect, ForceMode.Impulse);
        }

        gameObject.AddComponent<NotSeeObjectDelete>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Boss") return;
        if (obstacle == null || obstacle.enabled == true) return;

        var random = new System.Random();
        var min = 1;
        var max = 3;

        Rigidbody rd = GetComponent<Rigidbody>();
        BoxCollider collider = GetComponent<BoxCollider>();

        if (rd != null && collider != null)
        {
            rd.isKinematic = false;

            var vect = transform.position - other.transform.position;
            vect = vect.normalized * random.Next(min, max);
            rd.AddForce(vect, ForceMode.Impulse);
            rd.AddTorque(vect, ForceMode.Impulse);
        }

        gameObject.AddComponent<NotSeeObjectDelete>();
    }
}
