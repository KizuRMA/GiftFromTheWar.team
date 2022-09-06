using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    public void OnClickStartButton()
    {
        if (TimeAttackManager.Instance.timeAttackFlg) TimeAttackManager.Instance.TimerFinish();

        //StartCoroutine(LoadManager.Instance.LoadScene("Scenes/TimeAttackSelect"));

        SceneManager.LoadScene("Scenes/TimeAttackSelect");
    }
}
