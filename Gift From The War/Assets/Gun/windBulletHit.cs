using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windBulletHit : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnHitAttack(Collider _collider)
    {
        if (_collider.tag != "Bat") return;

        var target = _collider.transform.parent.GetComponent<BatController>();
        if (null == target) return;

        target.Damage(1);
    }
}
