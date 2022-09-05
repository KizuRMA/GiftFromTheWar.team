using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRanking : MonoBehaviour
{
    public int rankingType;

    public enum RankingType
    {
        FirstStage, SecondStage, FinalStage, ALL
    }

    // Start is called before the first frame update
    void Start()
    {
        rankingType = (int)RankingType.FirstStage;    
    }

    public void SelectFirst()
    {
        rankingType = (int)RankingType.FirstStage;
    }
    public void SelectSecond()
    {
        rankingType = (int)RankingType.SecondStage;
    }
    public void SelectFinal()
    {
        rankingType = (int)RankingType.FinalStage;
    }
    public void SelectALL()
    {
        rankingType = (int)RankingType.ALL;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
