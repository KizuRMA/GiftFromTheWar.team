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

        if (AchievementManager.Instance != null && !AchievementManager.Instance.nowAchievementData.badData && !TimeAttackManager.Instance.timeAttackFlg)
        {
            AchievementManager.Instance.nowAchievementData.badData = true;
            AchievementManager.Instance.WriteFile();
        }

        if (TimeAttackManager.Instance.nowStage == TimeAttackManager.selectStage.ALL || !TimeAttackManager.Instance.timeAttackFlg)
        {
            if(LoadManager.Instance != null)
            StartCoroutine(LoadManager.Instance.LoadScene("Scenes/SecondStage"));

            // ストーリー
            if (ScenarioData.Instance != null&& ScenarioData.Instance.saveData.neziKunCount==ScenarioData.Instance.saveData.maxCount)
            {
                ScenarioData.Instance.saveData.neziKunCount = 0;
                ScenarioData.Instance.WriteFile();
            }
            else if(ScenarioData.Instance!=null&&ScenarioData.Instance.saveData.neziKunCount<ScenarioData.Instance.saveData.maxCount)
            {
                ScenarioData.Instance.enable = false;
               
            }
        }
        else
        {
            TimeAttackManager.Instance.timerStopFlg = true;
            TimeAttackManager.Instance.timerStartFlg = false;
            TimeAttackManager.Instance.TimerHide();
            CursorManager.Instance.cursorLock = false;
            SceneManager.LoadScene("TimeAttackResult");
        }
    }
}
