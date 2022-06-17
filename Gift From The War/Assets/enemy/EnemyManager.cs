using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum e_EnemyType
{
    Bat,
    PatrolBat,
    Dog,
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemySpawnList list = null;
    [SerializeField] public GameObject player = null;
    [SerializeField] public WayPoint wayPoints = null;

    private void Awake()
    {
        EnemySpawn enemy = list.spawnLists[0];

        int listTypeSize = enemy.spawnTypeLists.Count;
        int listPosSize = enemy.spawnPosLists.Count;

        if (listTypeSize != listPosSize) return;

        for (int i = 0; i < listTypeSize; i++)
        {
            GameObject game = Instantiate(enemy.spawnTypeLists[i]);
            EnemyInterface info = game.GetComponent<EnemyInterface>();
            info.EnemySpawn(enemy.spawnPosLists[i].position);
            info.EnemyInfo(this);
        }
    }
}
