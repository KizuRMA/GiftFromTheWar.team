using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundText : MonoBehaviour
{
    Slider slider;

    private float textNum;

    public GameObject textObject = null;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();

        Text text = textObject.GetComponent<Text>();
        textNum = 100;
        text.text = "" + textNum.ToString("N0");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Method()
    {
        Text text = textObject.GetComponent<Text>();
        //slider.value��0�`1�͈̔͂Ȃ���
        //�e�L�X�g��0�`100�͈̔͂ɂ���
        textNum = slider.value * 100;

        text.text = "" + textNum.ToString("N0");
    }
}
