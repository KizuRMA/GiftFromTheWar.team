using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windBulletHit : MonoBehaviour
{
    public void OnTriggerStay(Collider _collider)
    {
        if (_collider.tag == "Detector") return;

        Destroy(this.gameObject);

        if (_collider.tag != "Bat") return;

        if (_collider.gameObject.tag == "Bat")
        {
            var target = _collider.transform.GetComponent<EnemyInterface>();
            target.Damage(1);
        }

        return;
    }
}
