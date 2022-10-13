using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextScene : MonoBehaviour
{
    public void OnClickStartButton()
    {
        StartCoroutine(LoadManager.Instance.LoadScene("Scenes/FirstScene"));
        SaveManager.Instance.Restart();
        SaveManager.Instance.WriteFile();
        SaveManager.Instance.WriteSubFile();

        //ストーリー
        ScenarioData.Instance.saveData.neziKunCount = 0;
        ScenarioData.Instance.saveData.talkCount = 0;
        ScenarioData.Instance.WriteFile();

    }
}
