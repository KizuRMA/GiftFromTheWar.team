using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class DogManager : BaseEnemyManager
{


    [SerializeField] public GameObject prefab = null;
    [SerializeField] public float respawnInterval;
    [SerializeField] public List<Transform> respawnPos = null;
    [SerializeField] public List<Transform> startPosList = null;
    [SerializeField] public HandGimmick gimmick = null;
    [SerializeField] public Transform warpPos = null;

    //子の配列番号と、ゲームオブジェクトのMAPリストを使用
    private List<GameObject> objects = new List<GameObject>();
    private Dictionary<string, int> agentTypeIdDict = new Dictionary<string, int>();

    //生存しているすべての犬を管理する
    public List<GameObject> dogs = null;

    bool isResetPriority;
    int warpCount = 0;

    EnemyManager owner;

    public bool IsRespawn
    {
        get
        {
            return (dogs.Count + respawnPlan) < enemyMax;
        }
    }

    private void Awake()
    {
        owner = transform.parent.GetComponent<EnemyManager>();
        isResetPriority = false;

        //NavMeshのAgentの種類を可変長配列に記録する
        for (var i = 0; i < NavMesh.GetSettingsCount(); i++)
        {
            var id = NavMesh.GetSettingsByIndex(i).agentTypeID;
            var name = NavMesh.GetSettingsNameFromID(id);
            agentTypeIdDict.Add(name, id);
        }

        //==========================================================
        // 犬が生成される前から配置されている犬に必要な情報を入れる
        //==========================================================

        GameObject[] _ChildObjects = GetChildObjects();

        for (int i = 0; i < _ChildObjects.Length; i++)
        {
            //敵の基本情報
            EnemyInterface info = _ChildObjects[i].GetComponent<EnemyInterface>();
            owner.SwitchManager(info.enemyType);
            info.EnemyInfo(owner);
        }
    }

    private void Start()
    {
        //子オブジェクトを全て取得する
        GameObject[] _ChildObjects = GetChildObjects();

        //配置されている最大数を調べて記録する
        for (int i = 0; i < _ChildObjects.Length; i++)
        {
            dogs.Add(_ChildObjects[i]);
            EnemyCounter();
        }
    }

    void Update()
    {
        if (!isResetPriority)
        {
            ResetGimmick();
            ResetPriority();
            isResetPriority = true;
        }

        //敵リスポーン
        EnemyReSpawn();

        ResetAgentType();
    }

    protected override void EnemyReSpawn()
    {
        int[] deleteNum = new int[dogs.Count];
        Array.Fill(deleteNum, -1);

        //配列を消す
        for (int i = 0; i < dogs.Count; i++)
        {
            DogState _state = dogs[i].GetComponent<DogState>();
            if (_state == null) continue;

            if (_state.IsCurrentState(e_DogState.BlowedAway) == true)
            {
                deleteNum[i] = i;
            }
        }

        foreach (var array in deleteNum)
        {
            if (array >= deleteNum.Length) break;

            if (array != -1)
            {
                dogs.RemoveAt(array);
            }
        }

        if (!IsRespawn) return;

        //敵をリスポーンさせる
        StartCoroutine(RespawnCoroutine());
    }

    public void ResetPriority()
    {
        for (int i = 0; i < dogs.Count; i++)
        {
            NavMeshAgent _agent = dogs[i].GetComponent<NavMeshAgent>();
            if (_agent == null) continue;

            _agent.avoidancePriority = 50;
            _agent.avoidancePriority += i;
        }
    }

    protected override bool IsChasing()
    {
        for (int i = 0; i < dogs.Count; i++)
        {
            DogState _state = dogs[i].GetComponent<DogState>();
            if (_state == null) continue;

            if (_state.IsChasing() == true)
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator RespawnCoroutine()
    {
        RespawnPlanCounter(1);

        yield return new WaitForSeconds(respawnInterval);

        GameObject game = Instantiate(prefab);
        EnemyInterface info = game.GetComponent<EnemyInterface>();

        //プレイヤーから最も遠いリスポーン地点を調べる
        int _respawnIndex = 0;
        float _maxDis = 0;
        Vector3 _playerPos = owner.player.transform.position;
        for (int i = 0; i < respawnPos.Count; i++)
        {
            float _dis = Vector2.Distance(new Vector2(respawnPos[i].position.x, respawnPos[i].position.z), new Vector2(_playerPos.x, _playerPos.z));
            if (_dis > _maxDis)
            {
                _maxDis = _dis;
                _respawnIndex = i;
            }
        }

        info.EnemySpawn(respawnPos[_respawnIndex].position);
        info.EnemyInfo(owner);
        game.transform.parent = this.transform;
        dogs.Add(game);

        RespawnPlanCounter(-1);
        ResetStartPos();
        ResetPriority();

        //ボタンをリセット
        ResetButton();
        SetButton(game);
    }

    private void ResetStartPos()
    {
        for (int i = 0; i < dogs.Count; i++)
        {
            DogState _state = dogs[i].GetComponent<DogState>();
            if (_state == null || startPosList[i] == null) continue;

            _state.SetStartPos(startPosList[i].transform.position);
        }
    }

    private void ResetGimmick()
    {
        if (gimmick == null) return;

        //生存していて、ボタンを背負っている犬が
        for (int i = 0; i < dogs.Count; i++)
        {
            if (dogs[i] == null) continue;
            SetButton(dogs[i]);
        }
    }

    private void SetButton(GameObject _game)
    {
        if (_game == null || gimmick == null) return;

        HandButton _button = _game.transform.Find("DogButton 1").GetComponent<HandButton>();

        if (_button == null) return;

        if (gimmick.button1 == null)
        {
            gimmick.HandButtonChange(ref gimmick.button1, ref _button);
        }
        else if (gimmick.button2 == null)
        {
            gimmick.HandButtonChange(ref gimmick.button2, ref _button);
        }
        else if (gimmick.button3 == null)
        {
            gimmick.HandButtonChange(ref gimmick.button3, ref _button);
        }
    }

    public void ResetButton()   //既にセットされている機能してないボタンを除く
    {
        if (gimmick == null) return;

        if (gimmick.button1 != null)
        {
            //ボタンが犬から離れていて、押されてないなら
            if (gimmick.button1.transform.parent == null && gimmick.button1.changeFlg == false)
            {
                gimmick.HandButtonDelete(ref gimmick.button1);
            }
        }

        if (gimmick.button2 != null)
        {
            if (gimmick.button2.transform.parent == null && gimmick.button2.changeFlg == false)
            {
                gimmick.HandButtonDelete(ref gimmick.button2);
            }
        }

        if (gimmick.button3 != null)
        {
            if (gimmick.button3.transform.parent == null && gimmick.button3.changeFlg == false)
            {
                gimmick.HandButtonDelete(ref gimmick.button3);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player" || warpPos == null || warpCount >= 2) return;

        for (int i = 0; i < dogs.Count; i++)
        {
            DogState _state = dogs[i].GetComponent<DogState>();
            if (_state == null) continue;

            Transform playerTrans = owner.player.transform;

            float dist = Vector2.Distance(new Vector2(_state.transform.position.x, _state.transform.position.z),
                new Vector2(playerTrans.position.x, playerTrans.position.z));

            if (_state.IsCurrentState(e_DogState.Search) == true &&
                dist >= 20.0f)
            {
                _state.WarpPosition(warpPos.position);
                warpCount++;
            }

            if (warpCount >= 2) return;
        }

    }

    private void ResetAgentType()
    {
        List<GameObject> trackList = new();
        List<GameObject> searchList = new();


        //現在管理している敵のAgentTypeを変更する
        for (int i = 0; i < dogs.Count; i++)
        {
            DogState _state = dogs[i].GetComponent<DogState>();
            if (_state == null) continue;

            if (_state.IsChasing() == true)
            {
                trackList.Add(dogs[i]);
            }
            else
            {
                searchList.Add(dogs[i]);
            }
        }

        for (int i = 0; i < trackList.Count; i++)
        {
            NavMeshAgent navMesh = trackList[i].GetComponent<NavMeshAgent>();

            switch (i % 2)
            {
                case 0:
                    navMesh.agentTypeID = agentTypeIdDict["DogAgent"];
                    break;
                case 1:
                    navMesh.agentTypeID = agentTypeIdDict["DogAgent2"];
                    break;
            }
        }

        for (int i = 0; i < searchList.Count; i++)
        {
            NavMeshAgent navMesh = searchList[i].GetComponent<NavMeshAgent>();

            switch (i % 2)
            {
                case 0:
                    navMesh.agentTypeID = agentTypeIdDict["DogAgent"];
                    break;
                case 1:
                    navMesh.agentTypeID = agentTypeIdDict["DogAgent2"];
                    break;
            }
        }

        trackList.Clear();
        searchList.Clear();
    }
}
