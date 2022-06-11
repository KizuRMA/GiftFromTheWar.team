using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windBulletHit : MonoBehaviour
{
    public void OnTriggerStay(Collider _collider)
    {
        Debug.Log(_collider.gameObject.name);
        if (_collider.tag == "Detector") return;

        Destroy(this.gameObject);

        if (_collider.tag != "Bat") return;

        Debug.Log(_collider.gameObject.name);

        if (_collider.gameObject.name == "Bat")
        {
            var target = _collider.transform.GetComponent<BatController>();
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
