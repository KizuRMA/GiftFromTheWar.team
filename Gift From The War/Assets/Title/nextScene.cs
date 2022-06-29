using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextScene : MonoBehaviour
{
    public void OnClickStartButton()
    {
        StartCoroutine(LoadManager.Instance.LoadScene("Scenes/MutukiScene"));
        SaveManager.Instance.Restart();
        SaveManager.Instance.WriteFile();
    }
}
