using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrenadeScript : MonoBehaviour
{
    [SerializeField] public float expRange;
    [SerializeField] public GameObject expParticle;

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.tag == "Player" || _collision.gameObject.tag == "cave")
        {
            GameObject _player = GameObject.Find("player");

            float dis = Vector3.Distance(_player.transform.position, transform.position);

            if (dis <= expRange)
            {
                var target = _player.transform.GetComponent<playerAbnormalcondition>();
                if (null == target) return;

                //target.Damage(10.0f);
            }
        }

        Instantiate(expParticle, transform.position,transform.rotation);
        Destroy(gameObject);
    }
}
