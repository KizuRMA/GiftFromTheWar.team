using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bom : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Boss") return;

        GameObject game = Instantiate(prefab, transform);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Boss") return;

        GameObject game = Instantiate(prefab, transform.position,transform.rotation);
        game.transform.localScale = transform.localScale;
        Destroy(gameObject);
    }
}
