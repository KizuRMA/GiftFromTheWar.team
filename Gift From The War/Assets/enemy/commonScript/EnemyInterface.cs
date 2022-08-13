using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyInterface : MonoBehaviour
{
    [SerializeField] public e_EnemyType enemyType;
    [SerializeField] private EnemyDamageEvent onDamage = new EnemyDamageEvent();
    [SerializeField] private EnemyMagnetCatch onMagnetCatch = new EnemyMagnetCatch();
    [SerializeField] private EnemyMagnetCatch onLavaDamage = new EnemyMagnetCatch();
    [SerializeField] private EnemyExplosionDamage onExpDamge = new EnemyExplosionDamage();
    [SerializeField] private  EnemySpawnPos onSpawn = new EnemySpawnPos();
    [SerializeField] private  EnemySerializeInfo oninfo = new EnemySerializeInfo();


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

    public void LavaDamage()
    {
        onLavaDamage.Invoke();
    }

    public void EnemySpawn(Vector3 _pos)
    {
        onSpawn.Invoke(_pos);
    }

    public void EnemyInfo(EnemyManager _info)
    {
        oninfo.Invoke(_info);
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

    [Serializable]
    public class EnemySerializeInfo : UnityEvent<EnemyManager>
    {

    }
}
