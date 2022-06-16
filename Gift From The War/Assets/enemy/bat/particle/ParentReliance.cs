using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentReliance : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    ParticleSystem myParticle;

    private void Awake()
    {
        myParticle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (particle == null) return;

        var main = particle.main;
        var myMain = myParticle.main;
        myMain.startLifetime = main.startLifetime.constant;
        myMain.startSize = main.startSize.constant;
        //myMain.duration = main.duration;
    }
}
