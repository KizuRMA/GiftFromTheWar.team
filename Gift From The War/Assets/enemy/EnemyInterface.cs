using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyInterface : MonoBehaviour
{
    [SerializeField] private EnemyDamageEvent onDamage = new EnemyDamageEvent();
    [SerializeField] private EnemyMagnetCatch onMagnetCatch = new EnemyMagnetCatch();
    [SerializeField] private EnemyExplosionDamage onExpDamge = new EnemyExplosionDamage();

    public void Damage(int _damage)
    {
        onDamage.Invoke(_damage);
    }

    public void MagnetCatch()
    {
        onMagnetCatch.Invoke();
    }

    public void ExpDamage(int _damage,Vector3 _pos)
    {
        onExpDamge.Invoke(_damage, _pos);
    }

    [Serializable]
    public class EnemyDamageEvent : UnityEvent<int>
    {

    }

    [Serializable]
    public class EnemyMagnetCatch : UnityEvent
    {

    }

    [Serializable]
    public class EnemyExplosionDamage : UnityEvent<int,Vector3>
    {

    }

}
