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


        GameObject game = Instantiate(expRangeObj, transform.position, transform.rotation);
        Destroy(game,3.0f);
        AudioManager.Instance.PlaySE("Grenade", game, isLoop: false, vol: 1);
        Destroy(gameObject);
    }
}
