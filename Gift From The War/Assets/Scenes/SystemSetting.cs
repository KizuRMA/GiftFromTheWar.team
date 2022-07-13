using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSetting : MonoBehaviour
{
    [SerializeField] int FrameRate = 60;

    private void Start()
    {
        DontDestroyOnLoad(this);
        Screen.SetResolution(Screen.width, Screen.height, true);
        //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        Application.targetFrameRate = FrameRate; // 60fpsÇ…ê›íË
    }
}
