using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BGMSlider : MonoBehaviour
{
    private Slider slider;

    void Start()
    {
        slider = this.GetComponent<Slider>();

        //スライダー現在値の設定
        slider.value = AudioManager.Instance.GetBGMVolume();

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            slider.value = AudioManager.Instance.GetBGMVolume();
        }

        AudioManager.Instance.ChangeBGMVolume(slider.value);
    }
}
