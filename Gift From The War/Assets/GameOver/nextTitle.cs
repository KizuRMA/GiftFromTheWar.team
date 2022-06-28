using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextTitle : MonoBehaviour
{
    public void OnClickStartButton()
    {
        LoadManager.Instance.LoadScene("Scenes/TitleScene");
    }
}
