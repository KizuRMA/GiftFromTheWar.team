using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SESlider : MonoBehaviour
{
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();

        //スライダー現在値の設定
        slider.value = AudioManager.Instance.GetSEVolume();

    }

    void Update()
    {
        AudioManager.Instance.ChangeSEVolume(slider.value);
    }
}
