using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SESlider : MonoBehaviour
{
    private Slider slider;

    void Start()
    {
        slider = this.GetComponent<Slider>();

        //�X���C�_�[���ݒl�̐ݒ�
        slider.value = AudioManager.Instance.GetSEVolume();

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            slider.value = AudioManager.Instance.GetSEVolume();
        }

        AudioManager.Instance.ChangeSEVolume(slider.value);
    }
}
