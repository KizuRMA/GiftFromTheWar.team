using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackRange : MonoBehaviour
{
    [SerializeField] public LayerMask layer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var target = other.transform.GetComponent<playerAbnormalcondition>();
            if (null == target) return;

            //“–‚½‚è”»’è
            Vector3 _firePos = transform.position;
            Vector3 _targetVec = (other.transform.position) - _firePos;

            //ƒŒƒC”»’è
            Ray _ray = new Ray(_firePos, _targetVec);
            RaycastHit _raycastHit;

            bool hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, layer);

            if (hit == false || _raycastHit.collider.tag != "Player") return;

            target.Damage(10.0f);
        }

        if (other.gameObject.tag == "Bat")
        {
            var target = other.transform.GetComponent<EnemyInterface>();
            if (null == target) return;

            target.Damage(5);
        }

    }
}
