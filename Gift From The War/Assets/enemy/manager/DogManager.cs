using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogManager : BaseEnemyManager
{
    //子の配列番号と、ゲームオブジェクトのMAPリストを使用
    private List<GameObject> objects = new List<GameObject>();
    private Dictionary<string, int> agentTypeIdDict = new Dictionary<string, int>();

    bool isResetPriority;

    private void Awake()
    {
        isResetPriority = false;
        for (var i = 0; i < NavMesh.GetSettingsCount(); i++)
        {
            var id = NavMesh.GetSettingsByIndex(i).agentTypeID;
            var name = NavMesh.GetSettingsNameFromID(id);
            agentTypeIdDict.Add(name, id);
        }
    }

    void Update()
    {
        if (!isResetPriority)
        {
            ResetPriority();
            isResetPriority = true;
        }

        //子オブジェクトを全て取得する
        GameObject[] _ChildObjects = GetChildObjects();

        //追跡している犬を検索
        for (int i = 0; i < transform.childCount; i++)
        {
            DogState _state = _ChildObjects[i].GetComponent<DogState>();
            if (_state == null) continue;

            //現在参照している敵が追跡状態である時
            if (_state.IsCurrentState(e_DogState.Tracking) == true || _state.IsCurrentState(e_DogState.Search) == true)
            {
                //追跡状態の敵を管理している配列に現在参照している敵が入っていない時
                if (objects.Contains(_ChildObjects[i]) == false)
                {
                    //追加する
                    objects.Add(_ChildObjects[i]);
                }
            }
        }

        //現在管理している配列の敵のAgentTypeを変更する
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

}
