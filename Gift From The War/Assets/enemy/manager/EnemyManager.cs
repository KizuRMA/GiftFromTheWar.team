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
    [SerializeField] private List<EnemySpawnList> list = null;
    [SerializeField] public GameObject player = null;
    [SerializeField] public WayPoint wayPoints = null;
    [SerializeField] public DogTerritory territory = null;
    [System.NonSerialized] public BaseEnemyManager manager = null;

    private void Awake()
    {
        foreach (var enemyList in list)
        {
            int maxIndex = enemyList.spawnLists.Count - 1;
            if (maxIndex < 0) continue;

            int index = Random.Range(0, maxIndex + 1);
            EnemySpawn enemy = enemyList.spawnLists[index];

            int listTypeSize = enemy.spawnTypeLists.Count;
            int listPosSize = enemy.spawnPosLists.Count;

            if (listTypeSize != listPosSize) return;

            for (int i = 0; i < listTypeSize; i++)
            {
                GameObject game = Instantiate(enemy.spawnTypeLists[i]);
                EnemyInterface info = game.GetComponent<EnemyInterface>();
                SwitchManager(info.enemyType);
                info.EnemySpawn(enemy.spawnPosLists[i].position);
                info.EnemyInfo(this);
                manager.EnemyCounter();
                game.transform.parent = manager.transform;
            }
        }
    }

    public void SwitchManager(e_EnemyType _type)
    {
        switch (_type)
        {
            case e_EnemyType.Bat:
                manager = transform.Find("BatManager").GetComponent<BatManager>();
                break;
            case e_EnemyType.PatrolBat:
                manager = transform.Find("PatrolBatManager").GetComponent<PatrolBatManager>();
                break;
            case e_EnemyType.Dog:
                manager = transform.Find("DogManager").GetComponent<DogManager>();
                break;
        }
    }

    public bool IsChasing(e_EnemyType _type)
    {
        SwitchManager(_type);
        return manager.IsEnemysChasing();
    }
}
