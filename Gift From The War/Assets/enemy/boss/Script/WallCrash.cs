using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WallCrash : MonoBehaviour
{
    [SerializeField] NavMeshObstacle obstacle;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) == true)
        {
            if (obstacle == null) return;
            obstacle.enabled= false;
        }
    }
}
