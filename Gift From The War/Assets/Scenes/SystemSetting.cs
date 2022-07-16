using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSetting : SingletonMonoBehaviour<SystemSetting>
{
    [SerializeField] int FrameRate = 60;
    public bool topPriorityUI; //最優先で表示するUIが有効か無効

    private void Start()
    {
        topPriorityUI = false;
        DontDestroyOnLoad(this);
        Screen.SetResolution(Screen.width, Screen.height, true);
        Application.targetFrameRate = FrameRate; // 60fpsに設定
    }

    private void Update()
    {
        
    }

    public void Pause()
    {
        Time.timeScale = 0;  // 時間停止
    }

    public void Resume()
    {
        Time.timeScale = 1;  // 再開
    }
}
