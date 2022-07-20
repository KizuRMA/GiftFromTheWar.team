using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneTimeAttack : MonoBehaviour
{
    [SerializeField] private PlayerStartDown playerStart;

    void Start()
    {
        if (!TimeAttackManager.Instance.timeAttackFlg) return;

        TimeAttackManager.Instance.timerStartFlg = true;
        playerStart.isDebug = true;
    }

    void Update()
    {
        
    }
}
