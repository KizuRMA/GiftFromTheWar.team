using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeIcon : IconBase
{
    void Start()
    {
        
    }

    void Update()
    {
        if(AchievementManager.Instance.nowAchievementData.timeAttackAFlg == true)
        {
            UpAlpha();
        }
        else
        {
            DownAlpha();
        }
    }
}
