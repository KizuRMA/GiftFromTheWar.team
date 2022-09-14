using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSetting : SingletonMonoBehaviour<SystemSetting>
{
    public enum e_PauseType
    {
        None,
        Select,
        Document,
    }

    [SerializeField] int FrameRate = 60;
    [System.NonSerialized]public bool topPriorityUI; //Å—Dæ‚Å•\¦‚·‚éUI‚ª—LŒø‚©–³Œø
    [System.NonSerialized]public e_PauseType pauseType;

    private void Start()
    {
        pauseType = e_PauseType.None;
        topPriorityUI = false;
        DontDestroyOnLoad(this);
        Screen.SetResolution(Screen.width, Screen.height, true);
        Application.targetFrameRate = FrameRate; // 60fps‚Éİ’è
    }

    private void Update()
    {

    }

    public void Pause(e_PauseType _type)
    {
        if (pauseType != e_PauseType.None) return;
        pauseType = _type;
        Time.timeScale = 0;  // ŠÔ’â~
    }

    public void Resume()
    {
        pauseType = e_PauseType.None;
        Time.timeScale = 1;  // ÄŠJ
    }
}
