using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrenadeScript : MonoBehaviour
{
    [SerializeField] public GameObject expParticle;
    [SerializeField] public GameObject expRangeObj;


    private void OnCollisionEnter(Collision _collision)
    {
        Instantiate(expParticle, transform.position,transform.rotation);
        Destroy(Instantiate(expRangeObj, transform.position,transform.rotation),1.0f);
        Destroy(gameObject);
    }
}
