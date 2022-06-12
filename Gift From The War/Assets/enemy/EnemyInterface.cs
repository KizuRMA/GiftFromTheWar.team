using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyInterface : MonoBehaviour
{
    [SerializeField] private EnemyDamageEvent onDamage = new EnemyDamageEvent();
    [SerializeField] private EnemyMagnetCatch onMagnetCatch = new EnemyMagnetCatch();

    public void Damage(int damage)
    {
        onDamage.Invoke(damage);
    }

    public void MagnetCatch()
    {
        onMagnetCatch.Invoke();
    }



    [Serializable]
    public class EnemyDamageEvent : UnityEvent<int>
    {

    }

    [Serializable]
    public class EnemyMagnetCatch : UnityEvent
    {

    }


}
