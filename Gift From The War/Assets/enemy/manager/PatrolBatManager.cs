using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBatManager : BaseEnemyManager
{
    [SerializeField] public WayPointList wayPointLists;
    [SerializeField] public GameObject prefab = null;
    [SerializeField] public float respawnInterval;
    [SerializeField] public List<Transform> respawnPos;

    EnemyManager owner;
    int wayPointIndex;
    int wayPointIndexMax = 0;
  

    private void Awake()
    {
        wayPointIndex = 0;
        owner = transform.parent.GetComponent<EnemyManager>();
    }

    private void Start()
    {
        wayPointIndexMax = wayPointLists.wayPoints.Count;
        SetWayPointRoot();
        ResetPriority();
    }

    private void Update()
    {
        EnemyReSpawn();
    }

    protected override void EnemyReSpawn()
    {
        if (numberEnemies <= transform.childCount + numberRespawnPlan) return;

        //敵をリスポーンさせる
        StartCoroutine(RespawnCoroutine());
    }

    private void SetWayPointRoot()
    {
        //子オブジェクトを全て取得する
        GameObject[] _ChildObjects = GetChildObjects();


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
        RespawnPlanCounter(1);

        yield return new WaitForSeconds(respawnInterval);

        GameObject game = Instantiate(prefab);
        EnemyInterface info = game.GetComponent<EnemyInterface>();

        int _respawnIndex = 0;
        float _maxDis = 0;
        Vector3 _playerPos = owner.player.transform.position;
        for (int i = 0; i < respawnPos.Count; i++)
        {
            float _dis = Vector2.Distance(new Vector2(respawnPos[i].position.x, respawnPos[i].position.z),new Vector2(_playerPos.x, _playerPos.z));
            if (_dis > _maxDis)
            {
                _maxDis = _dis;
                _respawnIndex = i;
            }
        }

        info.EnemySpawn(respawnPos[_respawnIndex].position);
        info.EnemyInfo(owner);
        game.transform.parent = this.transform;

        RespawnPlanCounter(-1);
        SetWayPointRoot();
        ResetPriority();
    }

    public void ResetPriority() //コウモリの優先度をリセット
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

    protected override bool IsChasing()
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

            if (_state.IsChasing() == true)
            {
                return true;
            }
        }
        return false;
    }
}
