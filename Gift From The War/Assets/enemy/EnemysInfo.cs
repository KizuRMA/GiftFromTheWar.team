using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysInfo : MonoBehaviour
{
    [SerializeField] private List<EnemyManager> list = null;

    private void Update()
    {
        if (IsEnemysChasing(e_EnemyType.PatrolBat) == true || IsEnemysChasing(e_EnemyType.Bat) == true)
        {
            //AudioManager.Instance.PlaySE("Heartbeat");
            //AudioManager.Instance.PlayBGM("FANTASY-04");
        }
        else
        {
            //AudioManager.Instance.StopSE("Heartbeat");
            //AudioManager.Instance.PlayBGM("abc");
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
