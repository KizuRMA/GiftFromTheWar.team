using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class windBulletHit : MonoBehaviour
{
    private shooting shot;
    public GameObject targetImageObj = null;

    private void Start()
    {
        shot = GameObject.Find("muzzlePos").GetComponent<shooting>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Detector") return;

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

        shot.bulletTuochFlg = true;

        Destroy(this.gameObject);

        if (_collider.tag != "Bat") return;

        if (_collider.gameObject.tag == "Bat")
        {
            var target = _collider.transform.GetComponent<EnemyInterface>();
            target.Damage(1);

            if (targetImageObj != null)
            {
                targetImageObj.GetComponent<TargetSetting>().HitAnime();
            }
        }

        return;
    }
}
