using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindIcon : IconBase
{
    void Start()
    {
        
    }

    void Update()
    {
        if(AchievementManager.Instance.nowAchievementData.noWindFlg == true)
        {
            UpAlpha();
        }
        else
        {
            DownAlpha();
        }
    }
}
