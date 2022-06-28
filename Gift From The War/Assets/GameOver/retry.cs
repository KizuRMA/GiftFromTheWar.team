using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class retry : MonoBehaviour
{
    public void OnClickStartButton()
    {
        StartCoroutine(LoadManager.Instance.LoadScene("Scenes/SampleScene"));
        SaveManager.Instance.ReadFile();
    }
}
