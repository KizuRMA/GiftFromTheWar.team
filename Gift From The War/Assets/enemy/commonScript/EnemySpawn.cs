using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] public List<Transform> spawnLists = new List<Transform>();
    public int spawnNumber = 0;

    private void Awake()
    {
        if (spawnLists.Count > 0)
        {
            EnemyInterface enemy = GetComponent<EnemyInterface>();
            enemy.EnemySpawn(spawnLists[spawnNumber].position);
        }
    }
}
