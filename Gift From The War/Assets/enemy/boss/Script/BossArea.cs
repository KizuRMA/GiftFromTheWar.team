using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour
{
    [SerializeField] public BossState state;
    [SerializeField] public GameObject bossAttackRange;


    private void Start()
    {
        if (bossAttackRange != null)
            bossAttackRange.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boss")
        {
            state.attackFlg = true;
            state.trackingSpeed = 2.5f;
            state.agent.speed = state.trackingSpeed;
            if (bossAttackRange != null)
                bossAttackRange.SetActive(true);
        }
    }
}
