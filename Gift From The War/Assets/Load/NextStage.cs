using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
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

        if (!AchievementManager.Instance.nowAchievementData.badData)
        {
            AchievementManager.Instance.nowAchievementData.badData = true;
            AchievementManager.Instance.WriteFile();
        }

        if (!TimeAttackManager.Instance.timeAttackFlg)
        {
            StartCoroutine(LoadManager.Instance.LoadScene("Scenes/GameClearScene"));
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
