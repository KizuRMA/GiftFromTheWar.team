using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAttackFunction : MonoBehaviour
{
    [SerializeField] private DogState owner;
    [SerializeField] private Collider attackCollider;
    [SerializeField] private Rigidbody rd;
    public bool isJumpFlg;

    public void OnAttackStart()
    {
        attackCollider.enabled = true;
    }

    public void OnAttackFinished()
    {
        attackCollider.enabled = false;
    }

    public void OnHitAttack(Collider _collider) //�U�������������Ƃ��̏���
    {
        var target = _collider.GetComponent<playerAbnormalcondition>();
        if (null == target) return;
        target.Damage(1.0f);
    }

    public void AttackAnimeStart()
    {
        isJumpFlg = false;
    }

    public void Jump()
    {
        isJumpFlg = true;
        owner.animator.SetFloat("Speed", 1.8f);


        rd.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // �ˏo�p�x
        float angle = 30.0f;

        Transform _player = owner.player.transform;

        float _playerDis = Vector3.Distance(transform.position,_player.position);
        Vector3 _targetPos = transform.forward * (_playerDis * 1.5f) + transform.position;

        // �ˏo���x���Z�o
        Vector3 velocity = CalculateVelocity(transform.position, _targetPos, angle);

        Debug.Log(velocity);

        // �ˏo
        rd.AddForce(velocity * rd.mass, ForceMode.Impulse);
    }

    public void Air()
    {

    }

    public void Landing()
    {
        owner.animator.SetFloat("Speed", 0.9f);
    }

    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        // �ˏo�p�����W�A���ɕϊ�
        float rad = angle * Mathf.PI / 180;

        // ���������̋���x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // ���������̋���y
        float y = pointA.y - pointB.y;

        // �Ε����˂̌����������x�ɂ��ĉ���
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // �����𖞂����������Z�o�ł��Ȃ����Vector3.zero��Ԃ�
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }


}
