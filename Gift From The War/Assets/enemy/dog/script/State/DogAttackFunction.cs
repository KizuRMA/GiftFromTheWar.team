using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAttackFunction : MonoBehaviour
{
    [SerializeField] private DogState owner;
    [SerializeField] private Collider attackCollider;
    [SerializeField] private Rigidbody rd;

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
        owner.animator.SetFloat("Speed", 1.8f);

        rd.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Vector3 _upVec = transform.up.normalized * 3.5f;
        Vector3 _forwardVec = transform.forward.normalized * 4.5f;

        rd.AddForce(_upVec, ForceMode.Impulse);
        rd.AddForce(_forwardVec, ForceMode.Impulse);
    }

    public void Landing()
    {

    }

    public void Air()
    {

    }


}
