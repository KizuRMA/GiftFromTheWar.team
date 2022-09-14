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

    //�q�̔z��ԍ��ƁA�Q�[���I�u�W�F�N�g��MAP���X�g���g�p
    private List<GameObject> objects = new List<GameObject>();
    private Dictionary<string, int> agentTypeIdDict = new Dictionary<string, int>();

    //�������Ă��邷�ׂĂ̌����Ǘ�����
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

        //NavMesh��Agent�̎�ނ��ϒ��z��ɋL�^����
        for (var i = 0; i < NavMesh.GetSettingsCount(); i++)
        {
            var id = NavMesh.GetSettingsByIndex(i).agentTypeID;
            var name = NavMesh.GetSettingsNameFromID(id);
            agentTypeIdDict.Add(name, id);
        }

        //==========================================================
        // �������������O����z�u����Ă��錢�ɕK�v�ȏ�������
        //==========================================================

        GameObject[] _ChildObjects = GetChildObjects();

        for (int i = 0; i < _ChildObjects.Length; i++)
        {
            //�G�̊�{���
            EnemyInterface info = _ChildObjects[i].GetComponent<EnemyInterface>();
            owner.SwitchManager(info.enemyType);
            info.EnemyInfo(owner);
        }
    }

    private void Start()
    {
        //�q�I�u�W�F�N�g��S�Ď擾����
        GameObject[] _ChildObjects = GetChildObjects();

        //�z�u����Ă���ő吔�𒲂ׂċL�^����
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

        //�G���X�|�[��
        EnemyReSpawn();

        ResetAgentType();
    }

    protected override void EnemyReSpawn()
    {
        int[] deleteNum = new int[dogs.Count];
        Array.Fill(deleteNum, -1);

        //�z�������
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

        //�G�����X�|�[��������
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

        //�v���C���[����ł��������X�|�[���n�_�𒲂ׂ�
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

        //�{�^�������Z�b�g
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

        //�������Ă��āA�{�^����w�����Ă��錢��
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

    public void ResetButton()   //���ɃZ�b�g����Ă���@�\���ĂȂ��{�^��������
    {
        if (gimmick == null) return;

        if (gimmick.button1 != null)
        {
            //�{�^���������痣��Ă��āA������ĂȂ��Ȃ�
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


        //���݊Ǘ����Ă���G��AgentType��ύX����
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
