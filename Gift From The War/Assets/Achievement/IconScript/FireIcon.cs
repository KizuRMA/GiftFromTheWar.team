using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireIcon : IconBase
{
    void Start()
    {
        
    }

    void Update()
    {
        if(AchievementManager.Instance.nowAchievementData.noFireFlg == true)
        {
            UpAlpha();
        }
        else
        {
            DownAlpha();
        }
    }
}
