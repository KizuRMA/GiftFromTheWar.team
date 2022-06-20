using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBatManager : BaseEnemyManager
{
    [SerializeField] public WayPointList wayPointLists = new WayPointList();
    int wayPointIndex;
    int wayPointIndexMax = 0;

    private void Awake()
    {
        wayPointIndex = 0;
        wayPointIndexMax = wayPointLists.wayPoints.Count;
    }

    private void Start()
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
