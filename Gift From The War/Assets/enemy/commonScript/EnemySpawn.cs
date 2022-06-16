using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] public List<GameObject> spawnTypeLists = new List<GameObject>();
    [SerializeField] public List<Transform> spawnPosLists = new List<Transform>();

    private void Awake()
    {
        //if (spawnLists.Count > 0)
        //{
        //    EnemyInterface enemy = GetComponent<EnemyInterface>();
        //    enemy.EnemySpawn(spawnLists[spawnNumber].transform.position);
        //}
    }
}
