using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSetting : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private hand Hand;

    void Start()
    {
        image.enabled = false;
    }

    void Update()
    {
        if(SaveManager.Instance.nowSaveData.getGunFlg)
        {
            image.enabled = true;
        }

        if(Hand.GetHandFlg() || SystemSetting.Instance.topPriorityUI == true)
        {
            image.enabled = false;
        }
    }
}
