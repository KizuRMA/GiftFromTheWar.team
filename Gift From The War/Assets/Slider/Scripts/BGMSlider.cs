using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : MonoBehaviour
{
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();

        //�X���C�_�[���ݒl�̐ݒ�
        slider.value = AudioManager.Instance.GetBGMVolume();

    }

    void Update()
    {
        AudioManager.Instance.ChangeBGMVolume(slider.value);
    }
}
