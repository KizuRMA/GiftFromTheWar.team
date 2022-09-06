using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class retry : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SaveManager.Instance.ReadSubFile();
        SaveManager.Instance.WriteFile();

        if (TimeAttackManager.Instance.timeAttackFlg)
        {
            StartCoroutine(LoadManager.Instance.LoadScene("Scenes/FirstScene"));
            TimeAttackManager.Instance.timeAttackFlg = true;
            TimeAttackManager.Instance.timerStopFlg = false;
            TimeAttackManager.Instance.playerDiedFlg = false;
            TimeAttackManager.Instance.TimerReset();
            SaveManager.Instance.Restart();
            SaveManager.Instance.WriteFile();
        }
        else
        {           
            if (SaveManager.Instance.nowSaveData.saveSpotNum < SaveManager.SaveSpotNum.s2p1)
            {
                StartCoroutine(LoadManager.Instance.LoadScene("Scenes/FirstScene"));
            }
            else if(SaveManager.Instance.nowSaveData.saveSpotNum < SaveManager.SaveSpotNum.s3p1)
            {
                StartCoroutine(LoadManager.Instance.LoadScene("Scenes/SecondStage"));
            }
            else
            {
                StartCoroutine(LoadManager.Instance.LoadScene("Scenes/FinalStage"));
            }
        }
    }
}
