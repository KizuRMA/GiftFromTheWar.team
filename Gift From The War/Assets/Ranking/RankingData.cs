using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RankingData
{
    public string UserName;
    public double ClearTime;

    public static RankingData[] Deserialize(string json)
    {
        return JsonHelper.FromJson<RankingData>(json);
    }
}
