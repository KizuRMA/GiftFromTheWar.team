using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BGMSlider : MonoBehaviour
{
    private Slider slider;
    private float past;

    void Start()
    {
        slider = this.GetComponent<Slider>();

        //スライダー現在値の設定
        slider.value = AudioManager.Instance.GetBGMVolume();

        past = slider.value;

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            slider.value = AudioManager.Instance.GetBGMVolume();
        }

        if (past == slider.value) return;

        past = slider.value;
        AudioManager.Instance.ChangeBGMVolume(slider.value);
        AudioManager.Instance.WriteFile();
    }
}
