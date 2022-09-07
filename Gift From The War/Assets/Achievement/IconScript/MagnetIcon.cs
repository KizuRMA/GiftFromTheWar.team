using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetIcon : IconBase
{
    void Start()
    {
        
    }

    void Update()
    {
        if(AchievementManager.Instance.nowAchievementData.noMagnetFlg == true)
        {
            UpAlpha();
        }
        else
        {
            DownAlpha();
        }
    }
}
