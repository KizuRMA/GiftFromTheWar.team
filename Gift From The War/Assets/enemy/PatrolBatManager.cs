using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBatManager : BaseEnemyManager
{
    [SerializeField] public WayPointList wayPointLists = new WayPointList();
    [SerializeField] public GameObject prefab = null;
    [SerializeField] public List<Transform> respawnPos = new List<Transform>();

    EnemyManager owner;
    int wayPointIndex;
    int wayPointIndexMax = 0;
    bool respawnFlg;

    private void Awake()
    {
        respawnFlg = false;
        wayPointIndex = 0;
        wayPointIndexMax = wayPointLists.wayPoints.Count;
        owner = transform.parent.GetComponent<EnemyManager>();
    }

    private void Start()
    {
        SetWayPointRoot();
        ResetPriority();
    }

    private void Update()
    {
        EnemyReSpawn();  
    }

    protected override void EnemyReSpawn()
    {
        if (numberEnemies <= transform.childCount) return;
        if (respawnFlg == true) return;

        //敵をリスポーンさせる
        StartCoroutine(RespawnCoroutine());
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

    public IEnumerator RespawnCoroutine()
    {
        respawnFlg = true;

        yield return new WaitForSeconds(5.0f);

        GameObject game = Instantiate(prefab);
        EnemyInterface info = game.GetComponent<EnemyInterface>();
        info.EnemySpawn(respawnPos[0].position);
        info.EnemyInfo(owner);
        game.transform.parent = this.transform;

        SetWayPointRoot();
        ResetPriority();
        respawnFlg = false;
    }

    public void ResetPriority()
    {
        //子オブジェクトを全て取得する
        GameObject[] _ChildObjects = new GameObject[gameObject.transform.childCount];
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            _ChildObjects[i] = gameObject.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            NavMeshAgent _agent = _ChildObjects[i].GetComponent<NavMeshAgent>();
            if (_agent == null) continue;

            _agent.avoidancePriority = 50;
            _agent.avoidancePriority += i;
        }
    }
}
