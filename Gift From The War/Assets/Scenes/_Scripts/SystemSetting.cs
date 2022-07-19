using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSetting : SingletonMonoBehaviour<SystemSetting>
{
    [SerializeField] int FrameRate = 60;
    public bool topPriorityUI; //�ŗD��ŕ\������UI���L��������

    private void Start()
    {
        topPriorityUI = false;
        DontDestroyOnLoad(this);
        Screen.SetResolution(Screen.width, Screen.height, true);
        Application.targetFrameRate = FrameRate; // 60fps�ɐݒ�
    }

    private void Update()
    {
        
    }

    public void Pause()
    {
        Time.timeScale = 0;  // ���Ԓ�~
    }

    public void Resume()
    {
        Time.timeScale = 1;  // �ĊJ
    }
}
