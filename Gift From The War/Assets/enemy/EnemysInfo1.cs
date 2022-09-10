using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysInfo1 : MonoBehaviour
{
    [SerializeField] private string BGMName;
    [SerializeField] private float BGMVol;
    [SerializeField] private List<e_EnemyType> enemyType = null;
    [SerializeField] private List<EnemyManager> list = null;
    [SerializeField] private EnemysInfo1 enemyInfo;
    [SerializeField] private EnemysInfo1 enemyInfo2;
    public bool isChase = false;

    private void Update()
    {
        if (enemyInfo.isChase) return;
        if (enemyInfo2.isChase) return;

        isChase = false;

        foreach (var info in enemyType)
        {
            if (IsEnemysChasing(info) == true) isChase = true;
        }
        
        if (isChase)
        {
            AudioManager.Instance.PlaySE("Heartbeat");
            AudioManager.Instance.PlayBGM(BGMName, vol: BGMVol);
        }
        else
        {
            AudioManager.Instance.StopSE("Heartbeat");
            AudioManager.Instance.PlayBGM("abc");
        }
    }

    public bool IsEnemysChasing(e_EnemyType _type)
    {
        foreach (var info in list)
        {
            if (info == null) continue;

            if (info.IsChasing(_type) == true)
            {
                return true;
            }
        }

        return false;
    }
}
