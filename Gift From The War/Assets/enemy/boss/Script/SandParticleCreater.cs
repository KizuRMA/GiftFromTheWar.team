using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandParticleCreater : MonoBehaviour
{
    [SerializeField] public GameObject sandParticle;
    [SerializeField] public List<Transform> sandMakePos = new List<Transform>();

    public void RightFrontLeg()
    {
        if (sandMakePos.Count < 1) return;
        GameObject game = Instantiate(sandParticle, sandMakePos[0].position, transform.rotation);
        Destroy(game, 1.5f);
    }

    public void LeftFrontLeg()
    {
        if (sandMakePos.Count < 2) return;
        GameObject game = Instantiate(sandParticle, sandMakePos[1].position, transform.rotation);
        Destroy(game, 1.5f);
    }

    public void RightBackLeg()
    {
        if (sandMakePos.Count < 3) return;
        GameObject game = Instantiate(sandParticle, sandMakePos[2].position, transform.rotation);
        Destroy(game, 1.5f);
    }

    public void LeftBackLeg()
    {
        if (sandMakePos.Count < 4) return;
        GameObject game = Instantiate(sandParticle, sandMakePos[3].position, transform.rotation);
        Destroy(game,1.5f);
    }
}
