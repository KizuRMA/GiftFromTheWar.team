using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBatManager : BaseEnemyManager
{
    [SerializeField] public WayPointList wayPointLists = new WayPointList();
    [SerializeField] public GameObject prefab = null;
    [SerializeField] public Transform respawnPos = null;
    [SerializeField] public Transform respawnPos2 = null;

    EnemyManager owner;
    int wayPointIndex;
    int wayPointIndexMax = 0;

    private void Awake()
    {
        wayPointIndex = 0;
        wayPointIndexMax = wayPointLists.wayPoints.Count;
        owner = transform.parent.GetComponent<EnemyManager>();
    }

    private void Start()
    {
        SetWayPointRoot();
    }

    private void Update()
    {
        //EnemyReSpawn();
    }

    protected override void EnemyReSpawn()
    {
        if (numberEnemies <= transform.childCount) return;

        GameObject game = Instantiate(prefab);
        EnemyInterface info = game.GetComponent<EnemyInterface>();
        info.EnemySpawn(respawnPos.position);
        info.EnemyInfo(owner);
        game.transform.parent = this.transform;

        SetWayPointRoot();
    }

    private void SetWayPointRoot()
    {
        //子オブジェクトを全て取得する
        GameObject[] _ChildObjects = new GameObject[gameObject.transform.childCount];
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            _ChildObjects[i] = gameObject.transform.GetChild(i).gameObject;
        }


        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            BatPatrolState _state = _ChildObjects[i].GetComponent<BatPatrolState>();
            if (_state == null) continue;

            _state.SetWayPoint(wayPointLists.wayPoints[wayPointIndex]);
            wayPointIndex = (wayPointIndex + 1) % wayPointIndexMax;
        }
    }
}
