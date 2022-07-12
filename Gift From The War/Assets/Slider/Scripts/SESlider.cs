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

        //�X���C�_�[���ݒl�̐ݒ�
        slider.value = AudioManager.Instance.GetSEVolume();

    }

    void Update()
    {
        AudioManager.Instance.ChangeSEVolume(slider.value);
    }
}
