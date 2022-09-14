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
    [System.NonSerialized]public bool topPriorityUI; //最優先で表示するUIが有効か無効
    [System.NonSerialized]public e_PauseType pauseType;

    private void Start()
    {
        pauseType = e_PauseType.None;
        topPriorityUI = false;
        DontDestroyOnLoad(this);
        Screen.SetResolution(Screen.width, Screen.height, true);
        Application.targetFrameRate = FrameRate; // 60fpsに設定
    }

    private void Update()
    {

    }

    public void Pause(e_PauseType _type)
    {
        if (pauseType != e_PauseType.None) return;
        pauseType = _type;
        Time.timeScale = 0;  // 時間停止
    }

    public void Resume()
    {
        pauseType = e_PauseType.None;
        Time.timeScale = 1;  // 再開
    }
}
