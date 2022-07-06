using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyManager : MonoBehaviour
{
    protected int numberEnemies;

    public void EnemyCounter()
    {
        numberEnemies += 1;
    }

    protected virtual void EnemyReSpawn()   //敵をリスポーンさせる抽象化関数
    {

    }

    public bool IsEnemysChasing()
    {
        return IsChasing();
    }

    protected virtual bool IsChasing()
    {
        return false;
    }

    public GameObject[] GetEnemys()
    {
        return GetChildObjects();
    }

    protected GameObject[] GetChildObjects()
    {
        //子オブジェクトを全て取得する
        GameObject[] _ChildObjects = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _ChildObjects[i] = transform.GetChild(i).gameObject;
        }

        return _ChildObjects;
    }
}
