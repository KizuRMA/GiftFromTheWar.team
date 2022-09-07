using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour
{
    [SerializeField] public BossState state;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boss")
        {
            state.attackFlg = true;
            state.trackingSpeed = 2.5f;
            state.agent.speed = state.trackingSpeed;
        }
    }
}
