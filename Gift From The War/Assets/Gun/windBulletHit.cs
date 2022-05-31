using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windBulletHit : MonoBehaviour
{
    public void OnHitAttack(Collider _collider)
    {
        if (_collider.gameObject.tag == "dogAgent") return; //当たったのがdogAgentだったら処理しない

        Destroy(this.gameObject);

        if (_collider.tag != "Bat") return;

        var target = _collider.transform.parent.GetComponent<BatController>();
        if (null == target) return;

        target.Damage(1);
    }
}
