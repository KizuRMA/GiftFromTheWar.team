using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    Slider slider;


    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();

        float maxValue = 10f;
        float nowValue = 5f;

        //�X���C�_�[�ő�l�̐ݒ�
        slider.maxValue = maxValue;
        //�X���C�_�[���ݒl�̐ݒ�
        slider.value = nowValue;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Method()
    {
        Debug.Log("���݂̒l:" +slider.value);
    }
}
