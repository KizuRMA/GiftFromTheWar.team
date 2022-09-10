using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeAttackStart : MonoBehaviour
{
    [SerializeField] TimeAttackManager.selectStage thisStageNum;

    public void OnClickStartButton()
    {
        if (thisStageNum != TimeAttackManager.selectStage.NULL)
        {
            TimeAttackManager.Instance.nowStage = thisStageNum;
        }

        switch (TimeAttackManager.Instance.nowStage)
        {
            case TimeAttackManager.selectStage.FIRST:
                StartCoroutine(LoadManager.Instance.LoadScene("Scenes/FirstScene"));
                break;

            case TimeAttackManager.selectStage.SECOND:
                StartCoroutine(LoadManager.Instance.LoadScene("Scenes/SecondStage"));
                break;

            case TimeAttackManager.selectStage.FINAL:
                StartCoroutine(LoadManager.Instance.LoadScene("Scenes/FinalStage"));
                break;

            case TimeAttackManager.selectStage.ALL:
                StartCoroutine(LoadManager.Instance.LoadScene("Scenes/FirstScene"));
                break;
        }
       
        TimeAttackManager.Instance.timeAttackFlg = true;
        TimeAttackManager.Instance.timerStopFlg = false;
        TimeAttackManager.Instance.playerDiedFlg = false;
        TimeAttackManager.Instance.TimerReset();
        SaveManager.Instance.Restart();
        SaveManager.Instance.WriteFile();
    }
}
