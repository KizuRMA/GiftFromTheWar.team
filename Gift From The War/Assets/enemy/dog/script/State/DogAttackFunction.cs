using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAttackFunction : MonoBehaviour
{
    [SerializeField] private DogState owner;
    [SerializeField] private Collider attackCollider;

    public void OnAttackStart()
    {
        attackCollider.enabled = true;
    }

    public void OnAttackFinished()
    {
        attackCollider.enabled = false;
    }

    public void OnHitAttack(Collider _collider) //UŒ‚‚ª“–‚½‚Á‚½‚Æ‚«‚Ìˆ—
    {
        var target = _collider.GetComponent<playerAbnormalcondition>();
        if (null == target) return;

        target.Damage(1.0f);
    }
}
