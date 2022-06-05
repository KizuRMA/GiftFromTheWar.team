using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windBulletHit : MonoBehaviour
{
    public void OnTriggerStay(Collider _collider)
    {
        if (_collider.gameObject.tag == "dogAgent") return; //“–‚½‚Á‚½‚Ì‚ªdogAgent‚¾‚Á‚½‚çˆ—‚µ‚È‚¢
        Destroy(this.gameObject);

        if (_collider.tag != "Bat") return;

        if (_collider.gameObject.name == "CollisionDetector")
        {
            var target = _collider.transform.parent.GetComponent<BatController>();
            target.Damage(1);
        }
        else
        {
            var target = _collider.transform.GetComponent<BatPatrolState>();
            target.Damage(1);
        }
        return;
    }
}
