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
            Debug.Log("ホワイトアウト完了");
            SceneManager.LoadScene("Scenes/EndingScene");
        }
    }

    public void BloomSetting()
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

        if (!TimeAttackManager.Instance.timeAttackFlg)
        {
            Debug.Log("クリア地点だよ");

            bloomSet.bloomFlg = true;

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
