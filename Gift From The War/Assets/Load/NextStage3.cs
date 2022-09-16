using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class NextStage3 : MonoBehaviour
{
    private bool nextStageFlg;
    [SerializeField] private BloomSet bloomSet;

    void Start()
    {
        nextStageFlg = false;
    }

    void Update()
    {
        if (bloomSet.finishFlg)
        {
            SceneManager.LoadScene("Scenes/EndingScene");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        if (nextStageFlg) return;
        nextStageFlg = true;

        if (AchievementManager.Instance != null && !TimeAttackManager.Instance.timeAttackFlg)
        {
            //ボスのアチーブメント    
            if (!AchievementManager.Instance.nowAchievementData.bossData)
            {
                AchievementManager.Instance.nowAchievementData.bossData = true;
                AchievementManager.Instance.WriteFile();
            }

            //銃のアチーブメント
            if (!AchievementManager.Instance.nowAchievementData.noGunFlg && SaveManager.Instance.nowSaveData.getGunFlg == false)
            {
                AchievementManager.Instance.nowAchievementData.noGunFlg = true;
                AchievementManager.Instance.WriteFile();
            }

            //風のアチーブメント
            if (!AchievementManager.Instance.nowAchievementData.noWindFlg && SaveManager.Instance.nowSaveData.getWindFlg == false)
            {
                AchievementManager.Instance.nowAchievementData.noWindFlg = true;
                AchievementManager.Instance.WriteFile();
            }

            //磁石のアチーブメント
            if (!AchievementManager.Instance.nowAchievementData.noMagnetFlg && SaveManager.Instance.nowSaveData.getMagnetFlg == false)
            {
                AchievementManager.Instance.nowAchievementData.noMagnetFlg = true;
                AchievementManager.Instance.WriteFile();
            }

            //炎のアチーブメント
            if (!AchievementManager.Instance.nowAchievementData.noFireFlg && SaveManager.Instance.nowSaveData.getFireFlg == false)
            {
                AchievementManager.Instance.nowAchievementData.noFireFlg = true;
                AchievementManager.Instance.WriteFile();
            }
        }

        if (!TimeAttackManager.Instance.timeAttackFlg)
        {
            bloomSet.bloomFlg = true;
        }
        else
        {
            //タイムアタックのアチーブメント
            if (!AchievementManager.Instance.nowAchievementData.timeAttackAFlg && TimeAttackManager.Instance.countTime <= 1800.0f)
            {
                AchievementManager.Instance.nowAchievementData.timeAttackAFlg = true;
                AchievementManager.Instance.WriteFile();
            }

            TimeAttackManager.Instance.timerStopFlg = true;
            TimeAttackManager.Instance.timerStartFlg = false;
            TimeAttackManager.Instance.TimerHide();
            CursorManager.Instance.SetCursorLock(false);
            SceneManager.LoadScene("TimeAttackResult");
        }
    }
}
