using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBladeHit : MonoBehaviour
{
    [SerializeField] private float damage = 0.2f;

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Player")
        {
            playerAbnormalcondition player = other.GetComponent<playerAbnormalcondition>();
            player.Damage(damage);
        }
        AudioManager.Instance.PlaySE("BatCutTheWindCaveHit",gameObject,maxDistance:10.0f,isLoop:false,vol:0.2f);
    }
}
