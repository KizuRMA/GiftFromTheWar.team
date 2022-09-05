using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage2 : MonoBehaviour
{
    private bool nextStageFlg;

    void Start()
    {
        nextStageFlg = false;
    }

    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        if (nextStageFlg) return;
        nextStageFlg = true;

        if (AchievementManager.Instance != null && !AchievementManager.Instance.nowAchievementData.badData)
        {
            AchievementManager.Instance.nowAchievementData.badData = true;
            AchievementManager.Instance.WriteFile();
        }

        if (!TimeAttackManager.Instance.timeAttackFlg)
        {
            if(LoadManager.Instance != null)
            StartCoroutine(LoadManager.Instance.LoadScene("Scenes/FinalStage"));
        }
        else
        {
            SaveManager.Instance.nowSaveData = TimeAttackManager.Instance.saveData;
            SaveManager.Instance.WriteFile();
            TimeAttackManager.Instance.timerStopFlg = true;
            TimeAttackManager.Instance.timerStartFlg = false;
            TimeAttackManager.Instance.TimerHide();
            CursorManager.Instance.cursorLock = false;
            SceneManager.LoadScene("TimeAttackResult");
        }
    }
}
