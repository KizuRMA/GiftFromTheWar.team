using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class metalHitJudge : MonoBehaviour
{
    public bool hitJudge { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        hitJudge = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        hitJudge = false;
    }
}
