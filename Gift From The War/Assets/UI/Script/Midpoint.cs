using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Midpoint : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image target;
    [SerializeField] private hand Hand;
    [SerializeField] private PlayerStartDown player;

    void Start()
    {
        image.enabled = false;
    }

    void Update()
    {
        if (!SaveManager.Instance.nowSaveData.getGunFlg && player.isAuto == false)
        {
            image.enabled = true;
        }

        if (Hand.GetHandFlg() || target.enabled == true || SystemSetting.Instance.topPriorityUI == true)
        {
            image.enabled = false;
        }
    }
}
