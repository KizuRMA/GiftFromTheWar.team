using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatIcon : IconBase
{
    void Start()
    {
        
    }

    void Update()
    {
        if(AchievementManager.Instance.nowAchievementData.badData == true)
        {
            UpAlpha();
        }
        else
        {
            DownAlpha();
        }
    }
}
