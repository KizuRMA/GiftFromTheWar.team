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

        if (other.gameObject.tag == "Bat")
        {
            var target = other.transform.GetComponent<EnemyInterface>();
            target.Damage(1);
        }

        if (other.gameObject.tag == "Dog1")
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

        if (_collider.gameObject.tag == "Bat")
        {
            var target = _collider.transform.GetComponent<EnemyInterface>();
            target.Damage(1);

            if (targetImageObj != null)
            {
Å@              targetImageObj.GetComponent<TargetSetting>().HitAnime();
            }
        }

        if (_collider.gameObject.tag == "Dog1")
        {
            var target = _collider.transform.GetComponent<EnemyInterface>();

            if (targetImageObj != null)
            {
                targetImageObj.GetComponent<TargetSetting>().HitAnime(new Color(0.9921569f, 0.6321112f, 0,1));
            }
            target.Damage(1);
        }

        return;
    }
}
