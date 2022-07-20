using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeResult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;

    bool resultFlg;

    void Start()
    {
        resultFlg = false;
    }

    void Update()
    {
        if (resultFlg) return;

        resultFlg = false;

        //countTime‚©‚çŒo‰ßŠÔ‚ğZo
        float time = TimeAttackManager.Instance.Result();
        float countMinute = (int)time / 60;
        time = time % 60;
        float countSecond = time;

        timer.SetText(countMinute.ToString("00") + ":" + countSecond.ToString("00.00"));
    }
}
