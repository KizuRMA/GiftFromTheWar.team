using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatNavmeshBake : MonoBehaviour
{
    [SerializeField] NavMeshSurface[] navMesh;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GameObject.Find("stage").GetComponents<NavMeshSurface>();

        Debug.Log(NavMesh.GetSettingsNameFromID(navMesh[0].agentTypeID));
        Debug.Log(NavMesh.GetSettingsNameFromID(navMesh[1].agentTypeID));
    }

    // Update is called once per frame
    void Update()
    {
        //navMesh[0].BuildNavMesh();
    }
}
