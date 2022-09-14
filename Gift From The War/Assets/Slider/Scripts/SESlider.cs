using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SESlider : MonoBehaviour
{
    private Slider slider;
    private float past;

    void Start()
    {
        slider = this.GetComponent<Slider>();

        //�X���C�_�[���ݒl�̐ݒ�
        slider.value = AudioManager.Instance.GetSEVolume();

        past = slider.value;

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            slider.value = AudioManager.Instance.GetSEVolume();
        }

        if (past == slider.value) return;

        past = slider.value;
        AudioManager.Instance.ChangeSEVolume(slider.value);
        AudioManager.Instance.WriteFile();
    }
}
