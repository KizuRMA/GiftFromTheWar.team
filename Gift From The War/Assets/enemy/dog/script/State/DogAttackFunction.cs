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

    public void OnHitAttack(Collider _collider) //çUåÇÇ™ìñÇΩÇ¡ÇΩÇ∆Ç´ÇÃèàóù
    {
        var target = _collider.GetComponent<playerAbnormalcondition>();
        if (null == target) return;

        target.Damage(1.0f);
    }

    public void Jump()
    {
        Rigidbody rig = owner.dog.GetComponent<Rigidbody>();
        rig.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Vector3 _upVec = transform.up.normalized * 4;
        Vector3 _forwardVec = transform.forward.normalized * 2;

        rig.AddForce(_upVec, ForceMode.Impulse);
        rig.AddForce(_forwardVec, ForceMode.Impulse);
    }


}
