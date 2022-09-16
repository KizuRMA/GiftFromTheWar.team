using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrenadeScript : MonoBehaviour
{
    [SerializeField] public GameObject expParticle;
    [SerializeField] public GameObject expRangeObj;


    private void OnCollisionEnter(Collision _collision)
    {

        Instantiate(expParticle, transform.position, transform.rotation);
        GameObject game = Instantiate(expRangeObj, transform.position, transform.rotation);
        Destroy(game,1.0f);
        GameObject soundObj = new GameObject();
        soundObj.transform.position = game.transform.position;
        AudioManager.Instance.PlaySE("Grenade", soundObj, isLoop: false, vol: 1);
        Destroy(gameObject);
        Destroy(soundObj,3.0f);
    }
}
