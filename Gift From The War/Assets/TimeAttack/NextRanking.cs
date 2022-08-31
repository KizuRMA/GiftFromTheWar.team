using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextRanking : MonoBehaviour
{

    public void OnClickNextButton()
    {
        //if (TimeAttackManager.Instance.timeAttackFlg) TimeAttackManager.Instance.TimerFinish();

        StartCoroutine(LoadManager.Instance.LoadScene("Scenes/TimeAttackRanking"));
    }
}
