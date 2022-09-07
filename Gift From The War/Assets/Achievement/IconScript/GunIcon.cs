using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunIcon : IconBase
{
    void Start()
    {
        
    }

    void Update()
    {
        if(AchievementManager.Instance.nowAchievementData.noGunFlg == true)
        {
            UpAlpha();
        }
        else
        {
            DownAlpha();
        }
    }
}
