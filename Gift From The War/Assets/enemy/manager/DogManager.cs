using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogManager : BaseEnemyManager
{
    //子の配列番号と、ゲームオブジェクトのMAPリストを使用
    private List<GameObject> objects = new List<GameObject>();
    private Dictionary<string, int> agentTypeIdDict = new Dictionary<string, int>();

    [SerializeField] public GameObject prefab = null;
    [SerializeField] public float respawnInterval;
    [SerializeField] public List<Transform> respawnPos = null;
    [SerializeField] public List<Transform> startPosList = null;
    [SerializeField] public HandGimmick gimmick = null;

    public List<GameObject> dogs = null;

    bool isResetPriority;

    EnemyManager owner;

    private void Awake()
    {
        owner = transform.parent.GetComponent<EnemyManager>();

        isResetPriority = false;
        for (var i = 0; i < NavMesh.GetSettingsCount(); i++)
        {
            var id = NavMesh.GetSettingsByIndex(i).agentTypeID;
            var name = NavMesh.GetSettingsNameFromID(id);
            agentTypeIdDict.Add(name, id);
        }

        //既に子オブジェクトに犬が配置されている時
        GameObject[] _ChildObjects = GetChildObjects();
        EnemyManager _manager = transform.parent.GetComponent<EnemyManager>();

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject game = _ChildObjects[i];
            EnemyInterface info = game.GetComponent<EnemyInterface>();
            _manager.SwitchManager(info.enemyType);
            info.EnemyInfo(_manager);
            EnemyCounter();
            game.transform.parent = transform;
        }
    }

    private void Start()
    {
        //子オブジェクトを全て取得する
        GameObject[] _ChildObjects = GetChildObjects();

        //追跡している犬を検索
        for (int i = 0; i < transform.childCount; i++)
        {
            dogs.Add(_ChildObjects[i]);
        }
    }

    void Update()
    {
        if (!isResetPriority)
        {
            ResetButtonGimmick();
            ResetPriority();
            isResetPriority = true;
        }

        //敵リスポーン
        EnemyReSpawn();

        //現在管理している配列の敵のAgentTypeを変更する
        for (int i = 0; i < dogs.Count; i++)
        {
            NavMeshAgent navMesh = dogs[i].GetComponent<NavMeshAgent>();

            switch (i % 3)
            {
                case 0:
                    navMesh.agentTypeID = agentTypeIdDict["DogAgent"];
                    break;
                case 1:
                    navMesh.agentTypeID = agentTypeIdDict["DogAgent2"];
                    break;
                case 2:
                    break;
            }
        }
    }

    protected override void EnemyReSpawn()
    {

        //敵配列を消す
        for (int i = 0; i < dogs.Count; i++)
        {
            if (dogs[i] == null)
            {
                dogs.RemoveAt(i);
            }
        }

        if (numberEnemies <= dogs.Count + numberRespawnPlan) return;

        //敵をリスポーンさせる
        StartCoroutine(RespawnCoroutine());
    }

    public void ResetPriority()
    {
        //子オブジェクトを全て取得する
        GameObject[] _ChildObjects = GetChildObjects();

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
        GameObject[] _ChildObjects = GetChildObjects();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            DogState _state = _ChildObjects[i].GetComponent<DogState>();
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
        ResetButtonGimmick();
    }

    private void ResetStartPos()
    {
        //子オブジェクトを全て取得する
        GameObject[] _ChildObjects = GetChildObjects();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            DogState _state = _ChildObjects[i].GetComponent<DogState>();
            if (_state == null || startPosList == null) continue;

            _state.SetStartPos(startPosList[i].transform.position);
        }
    }

    private void ResetButtonGimmick()   //ボタンギミックの対応ギミックをリセットする
    {
        if (gimmick == null) return;

        for (int i = 0; i < transform.childCount; i++)
        {
            HandButton _button = dogs[i].transform.Find("DogButton 1").GetComponent<HandButton>();
            if (_button == null) continue;

            if (ButtonGimmickChange(ref gimmick.button1, ref _button)) { }
            else if (ButtonGimmickChange(ref gimmick.button2, ref _button)) { }
            else if (ButtonGimmickChange(ref gimmick.button3, ref _button)) { }
        }
    }

    private bool ButtonGimmickChange(ref HandButton gimmickButton, ref HandButton dogButton) //GimmickDoorに犬のボタンをセットする関数
    {
        if (gimmickButton == null)
        {
            gimmick.HandButtonChange(ref gimmickButton,ref dogButton);
            return true;
        }
        else
        {
            //親Objがある　かつ　ボタンがおされている時はセットし直さない
            if (gimmickButton.transform.parent != null && gimmickButton.changeFlg == true) return false;
            if (gimmick.button1 == null || gimmick.button2 == null || gimmick.button3 == null) return false;

            int count = 0;
            int buttonMax = 3;

            //生存していて、ボタンを背負っている犬が
            for (int i = 0; i < dogs.Count; i++)
            {
                if (dogs[i] == null) continue;

                DogState _state = dogs[i].GetComponent<DogState>();
                if (_state == null) continue;

                HandButton _button = dogs[i].transform.Find("DogButton 1").GetComponent<HandButton>();

                if (_state.IsAlive == true && _button != null)
                {
                    count++;
                }
            }

            if (count < buttonMax) return false;

            gimmick.HandButtonChange(ref gimmickButton, ref dogButton);
            return true;
        }
    }
}
