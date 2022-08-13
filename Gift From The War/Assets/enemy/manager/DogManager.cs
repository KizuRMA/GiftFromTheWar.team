using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogManager : BaseEnemyManager
{
    //�q�̔z��ԍ��ƁA�Q�[���I�u�W�F�N�g��MAP���X�g���g�p
    private List<GameObject> objects = new List<GameObject>();
    private Dictionary<string, int> agentTypeIdDict = new Dictionary<string, int>();

    [SerializeField] public GameObject prefab = null;
    [SerializeField] public float respawnInterval;
    [SerializeField] public List<Transform> respawnPos = null;
    [SerializeField] public List<Transform> startPosList = null;

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
    }

    private void Start()
    {
        //�q�I�u�W�F�N�g��S�Ď擾����
        GameObject[] _ChildObjects = GetChildObjects();

        //�ǐՂ��Ă��錢������
        for (int i = 0; i < transform.childCount; i++)
        {
            dogs.Add(_ChildObjects[i]);
        }
    }

    void Update()
    {
        if (!isResetPriority)
        {
            ResetPriority();
            isResetPriority = true;
        }

        //�G���X�|�[��
        EnemyReSpawn();

        //�q�I�u�W�F�N�g��S�Ď擾����
        GameObject[] _ChildObjects = GetChildObjects();

        //�ǐՂ��Ă��錢������
        for (int i = 0; i < transform.childCount; i++)
        {
            DogState _state = _ChildObjects[i].GetComponent<DogState>();
            if (_state == null) continue;

            //���ݎQ�Ƃ��Ă���G���ǐՏ�Ԃł��鎞
            if (_state.IsCurrentState(e_DogState.Tracking) == true || _state.IsCurrentState(e_DogState.Search) == true)
            {
                //�ǐՏ�Ԃ̓G���Ǘ����Ă���z��Ɍ��ݎQ�Ƃ��Ă���G�������Ă��Ȃ���
                if (objects.Contains(_ChildObjects[i]) == false)
                {
                    //�ǉ�����
                    objects.Add(_ChildObjects[i]);
                }
            }
        }

        //���݊Ǘ����Ă���z��̓G��AgentType��ύX����
        for (int i = 0; i < objects.Count; i++)
        {
            NavMeshAgent navMesh = objects[i].GetComponent<NavMeshAgent>();

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

        //�G�z�������
        for (int i = 0; i < dogs.Count; i++)
        {
            if (dogs[i] == null)
            {
                dogs.RemoveAt(i);
            }
        }

        if (numberEnemies <= dogs.Count + numberRespawnPlan) return;

        //�G�����X�|�[��������
        StartCoroutine(RespawnCoroutine());
    }

    public void ResetPriority()
    {
        //�q�I�u�W�F�N�g��S�Ď擾����
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
        //�q�I�u�W�F�N�g��S�Ď擾����
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

        RespawnPlanCounter(-1);
        ResetStartPos();
        ResetPriority();
    }

    private void ResetStartPos()
    {
        //�q�I�u�W�F�N�g��S�Ď擾����
        GameObject[] _ChildObjects = GetChildObjects();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            DogState _state = _ChildObjects[i].GetComponent<DogState>();
            if (_state == null || startPosList == null) continue;

            _state.SetStartPos(startPosList[i].transform.position);
        }

    }
}
