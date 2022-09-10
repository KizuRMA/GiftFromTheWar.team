using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyManager : MonoBehaviour
{
    protected int enemyMax;    //敵の数
    protected int respawnPlan;  //リスポーンする予定の数

    private void Awake()
    {
        enemyMax = 0;
        respawnPlan = 0;
    }

    public void EnemyCounter()
    {
        enemyMax += 1;
    }

    public void RespawnPlanCounter(int _num)
    {
        respawnPlan += _num;
        respawnPlan = Mathf.Max(respawnPlan, 0);
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
