using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WallCrash : MonoBehaviour
{
    [SerializeField] NavMeshObstacle obstacle;
    [System.NonSerialized] public bool openFlg = false;

    // Update is called once per frame
    void Update()
    {
        if (openFlg == false || obstacle == null) return;
        obstacle.enabled = false;
    }
}
