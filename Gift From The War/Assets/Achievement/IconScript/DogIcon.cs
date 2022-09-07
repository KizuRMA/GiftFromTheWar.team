using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogIcon : IconBase
{
    void Start()
    {
        
    }

    void Update()
    {
        if(AchievementManager.Instance.nowAchievementData.dogData == true)
        {
            UpAlpha();
        }
        else
        {
            DownAlpha();
        }
    }
}
