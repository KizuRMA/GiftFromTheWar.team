using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIcon : IconBase
{
    void Start()
    {
        
    }

    void Update()
    {
        if(AchievementManager.Instance.nowAchievementData.bossData == true)
        {
            UpAlpha();
        }
        else
        {
            DownAlpha();
        }
    }
}
