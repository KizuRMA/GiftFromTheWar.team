using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVolume : MonoBehaviour
{

    [SerializeField] string BGM;

    // Start is called before the first frame update
    void Start()
    {
        //�w�肳�ꂽ���y�𗬂�
        AudioManager.Instance.PlayBGM(BGM);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�X���C�h�o�[����
    public void SoundSliderOnValueChange(float newSliderValue)
    {
        // �X���C�h�o�[�ŉ��ʂ𒲐�
        AudioManager.Instance.ChangeVolume(newSliderValue, newSliderValue);
    }
}
