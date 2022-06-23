using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MetalDoorUseInfo : MonoBehaviour
{
    [SerializeField] public GameObject stage;
    [System.NonSerialized] public NavMeshSurface[] navSurfaces;

    private void Awake()
    {
        navSurfaces = stage.transform.GetComponents<NavMeshSurface>();
    }
}
