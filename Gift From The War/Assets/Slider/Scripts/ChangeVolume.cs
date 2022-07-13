using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVolume : MonoBehaviour
{

    [SerializeField] string BGM;

    // Start is called before the first frame update
    void Start()
    {
        //指定された音楽を流す
        AudioManager.Instance.PlayBGM(BGM);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //スライドバー操作
    public void SoundSliderOnValueChange(float newSliderValue)
    {
        // スライドバーで音量を調整
        AudioManager.Instance.ChangeVolume(newSliderValue, newSliderValue);
    }
}
