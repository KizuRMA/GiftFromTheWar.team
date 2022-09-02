using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextAchievement : MonoBehaviour
{

    public void OnClickNextButton()
    {
        StartCoroutine(LoadManager.Instance.LoadScene("Scenes/AchievementScene"));
    }
}
