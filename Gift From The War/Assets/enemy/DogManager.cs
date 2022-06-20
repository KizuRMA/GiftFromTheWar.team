using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogManager : BaseEnemyManager
{
    //子の配列番号と、ゲームオブジェクトのMAPリストを使用
    private List<GameObject> objects = new List<GameObject>();
    private Dictionary<string, int> agentTypeIdDict = new Dictionary<string, int>();

    private void Awake()
    {
        for (var i = 0; i < NavMesh.GetSettingsCount(); i++)
        {
            var id = NavMesh.GetSettingsByIndex(i).agentTypeID;
            var name = NavMesh.GetSettingsNameFromID(id);
            agentTypeIdDict.Add(name, id);
        }
    }

    void Update()
    {

        //子オブジェクトを全て取得する
        GameObject[] _ChildObjects = new GameObject[gameObject.transform.childCount];
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            _ChildObjects[i] = gameObject.transform.GetChild(i).gameObject;
        }

        //追跡している犬を検索
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            DogState _state = _ChildObjects[i].GetComponent<DogState>();
            if (_state == null) continue;

            //現在参照している敵が追跡状態である時
            if (_state.IsCurrentState(e_DogState.Tracking) == true)
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

        //管理している配列ないに追跡していない敵がいる場合
        for (int i = 0; i < objects.Count; i++)
        {
            DogState _state = objects[i].GetComponent<DogState>();
            if (_state == null) continue;

            //現在参照している敵が追跡状態でない時
            if (_state.IsCurrentState(e_DogState.Tracking) == false)
            {
                //エージェントタイプを変更して配列から削除する
                objects[i].GetComponent<NavMeshAgent>().agentTypeID = agentTypeIdDict["DogAgent"];
                objects.Remove(objects[i]);
            }
        }
    }
}
