using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyInterface : MonoBehaviour
{
    [SerializeField] private EnemyDamageEvent onDamage = new EnemyDamageEvent();
    [SerializeField] private EnemyMagnetCatch onMagnetCatch = new EnemyMagnetCatch();
    [SerializeField] private EnemyExplosionDamage onExpDamge = new EnemyExplosionDamage();
    [SerializeField] private  EnemySpawnPos onSpawn = new EnemySpawnPos();

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

    public void EnemySpawn(Vector3 _pos)
    {
        onSpawn.Invoke(_pos);
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

    [Serializable]
    public class EnemySpawnPos : UnityEvent<Vector3>
    {

    }

}
