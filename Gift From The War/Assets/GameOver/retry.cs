using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class retry : MonoBehaviour
{
    public void OnClickStartButton()
    {
        if(TimeAttackManager.Instance.timeAttackFlg)
        {
            StartCoroutine(LoadManager.Instance.LoadScene("Scenes/FirstScene"));
            TimeAttackManager.Instance.timeAttackFlg = true;
            TimeAttackManager.Instance.timerStopFlg = false;
            TimeAttackManager.Instance.playerDiedFlg = false;
            TimeAttackManager.Instance.TimerReset();
            TimeAttackManager.Instance.saveData = SaveManager.Instance.nowSaveData;
            SaveManager.Instance.Restart();
            SaveManager.Instance.WriteFile();
        }
        else
        {
            StartCoroutine(LoadManager.Instance.LoadScene("Scenes/FirstScene"));
            SaveManager.Instance.ReadFile();
        }
    }
}
