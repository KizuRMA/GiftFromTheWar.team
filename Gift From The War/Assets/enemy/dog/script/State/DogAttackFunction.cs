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
        rd.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Vector3 _upVec = transform.up.normalized * 4;
        Vector3 _forwardVec = transform.forward.normalized * 2;

        rd.AddForce(_upVec, ForceMode.Impulse);
        rd.AddForce(_forwardVec, ForceMode.Impulse);
    }

    public void Landing()
    {
        //if (rd.velocity.y < 0)
        //{
        //    owner.animator.speed = 0;
        //}
        //else
        //{
        //    owner.animator.speed = 1.0f;
        //}
    }

    public void Air()
    {
        //if (rd.velocity.y > 0)
        //{
        //    //Debug.Log("");
        //    owner.animator.speed = 0;
        //}
        //else
        //{
        //    owner.animator.speed = 1.0f;
        //}
    }


}
