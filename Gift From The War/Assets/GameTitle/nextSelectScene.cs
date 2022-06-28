using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextSelectScene : MonoBehaviour
{
    public void OnClickButton()
    {
        SceneManager.LoadScene("Scenes/TitleScene");
        SaveManager.Instance.Restart();
        SaveManager.Instance.WriteFile();
    }
}
