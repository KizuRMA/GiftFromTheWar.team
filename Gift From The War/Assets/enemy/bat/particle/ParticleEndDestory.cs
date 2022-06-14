using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEndDestory : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
