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

        //スライダー最大値の設定
        slider.maxValue = maxValue;
        //スライダー現在値の設定
        slider.value = nowValue;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Method()
    {
        Debug.Log("現在の値:" +slider.value);
    }
}
