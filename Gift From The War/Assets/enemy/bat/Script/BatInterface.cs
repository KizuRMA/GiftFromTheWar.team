using System;
using UnityEngine;
using UnityEngine.Events;

public class BatInterface : MonoBehaviour
{
    [SerializeField] private BatDamageEvent onDamage = new BatDamageEvent();

    public void Damage(int damage)
    {
        onDamage.Invoke(damage);
    }

    [Serializable]
    public class BatDamageEvent : UnityEvent<int>
    {

    }
}
