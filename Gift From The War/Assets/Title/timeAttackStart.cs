using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeAttackStart : MonoBehaviour
{
    public void OnClickStartButton()
    {
        StartCoroutine(LoadManager.Instance.LoadScene("Scenes/FirstScene"));
        TimeAttackManager.Instance.timeAttackFlg = true;
        TimeAttackManager.Instance.timerStopFlg = false;
        TimeAttackManager.Instance.playerDiedFlg = false;
        TimeAttackManager.Instance.TimerReset();

    }
}
