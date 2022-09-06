using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeAttackStart : MonoBehaviour
{
    [SerializeField] TimeAttackManager.selectStage thisStageNum;

    public void OnClickStartButton()
    {
        TimeAttackManager.Instance.nowStage = thisStageNum;

        switch ((int)TimeAttackManager.Instance.nowStage)
        {
            case 0:
                StartCoroutine(LoadManager.Instance.LoadScene("Scenes/FirstScene"));
                break;

            case 1:
                StartCoroutine(LoadManager.Instance.LoadScene("Scenes/SecondStage"));
                break;

            case 2:
                StartCoroutine(LoadManager.Instance.LoadScene("Scenes/FinalStage"));
                break;

            case 3:
                StartCoroutine(LoadManager.Instance.LoadScene("Scenes/FirstScene"));
                break;
        }
       
        TimeAttackManager.Instance.timeAttackFlg = true;
        TimeAttackManager.Instance.timerStopFlg = false;
        TimeAttackManager.Instance.playerDiedFlg = false;
        TimeAttackManager.Instance.TimerReset();
        TimeAttackManager.Instance.saveData = SaveManager.Instance.nowSaveData;
        SaveManager.Instance.Restart();
        SaveManager.Instance.WriteFile();
    }
}
