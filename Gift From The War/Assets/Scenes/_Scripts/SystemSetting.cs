using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSetting : SingletonMonoBehaviour<SystemSetting>
{
    [SerializeField] int FrameRate = 60;
    public bool topPriorityUI; //Å—Dæ‚Å•\¦‚·‚éUI‚ª—LŒø‚©–³Œø

    private void Start()
    {
        topPriorityUI = false;
        DontDestroyOnLoad(this);
        Screen.SetResolution(Screen.width, Screen.height, true);
        Application.targetFrameRate = FrameRate; // 60fps‚Éİ’è
    }

    private void Update()
    {
        
    }

    public void Pause()
    {
        Time.timeScale = 0;  // ŠÔ’â~
    }

    public void Resume()
    {
        Time.timeScale = 1;  // ÄŠJ
    }
}
