using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatManager : BaseEnemyManager
{
    private void Start()
    {
        //子オブジェクトを全て取得する
        GameObject[] _ChildObjects = GetChildObjects();

        //配置されている最大数を調べて記録する
        for (int i = 0; i < _ChildObjects.Length; i++)
        {
            EnemyCounter();
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
            BatController _state = _ChildObjects[i].GetComponent<BatController>();
            if (_state == null) continue;

            if (_state.IsChasing() == true)
            {
                return true;
            }
        }
        return false;
    }
}
