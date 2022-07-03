using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windBulletHit : MonoBehaviour
{
    private shooting shot;

    private void Start()
    {
        shot = GameObject.Find("muzzlePos").GetComponent<shooting>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Detector") return;

        Debug.Log(other.gameObject);

        shot.bulletTuochFlg = true;

        Destroy(this.gameObject);

        if (other.tag != "Bat") return;

        if (other.gameObject.tag == "Bat")
        {
            var target = other.transform.GetComponent<EnemyInterface>();
            target.Damage(1);
        }

        return;
    }

    public void OnTriggerStay(Collider _collider)
    {
        if (_collider.tag == "Detector") return;

        Debug.Log(_collider.gameObject);

        shot.bulletTuochFlg = true;

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
