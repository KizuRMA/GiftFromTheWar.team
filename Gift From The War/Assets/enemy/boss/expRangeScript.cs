using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expRangeScript : MonoBehaviour
{
    [SerializeField] public float damage;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                var target = other.gameObject.transform.GetComponent<playerAbnormalcondition>();
                if (null == target) return;
                target.Damage(damage);
                break;
            case "Bat":
                EnemyInterface bat = other.gameObject.transform.GetComponent<EnemyInterface>();
                if (bat == null) return;
                bat.ExpDamage(10,transform.position);
                break;
        }
    }
}
