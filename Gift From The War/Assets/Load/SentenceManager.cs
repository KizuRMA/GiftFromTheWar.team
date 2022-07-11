using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceManager : MonoBehaviour
{
    private List<GameObject> sentenceList;

    void Start()
    {
        sentenceList = new List<GameObject>();
        SelectSentence();
    }

    void Update()
    {
        
    }

    private void SelectSentence()
    {
        int spotNum = (int)SaveManager.Instance.nowSaveData.saveSpotNum + 1;
        int sumSentence = 0;    //表示される可能性がある全ての文章の合計

        for (int i = 0; i < spotNum; i++)
        {
            sumSentence += this.transform.GetChild(i).transform.childCount;

            for (int j = 0; j < this.transform.GetChild(i).transform.childCount; j++)
            {
                this.transform.GetChild(i).transform.GetChild(j).gameObject.SetActive(false);
                sentenceList.Add(this.transform.GetChild(i).transform.GetChild(j).gameObject);
            }
        }

        int rand = Random.Range(0, sumSentence);

        sentenceList[rand].SetActive(true);
        
    }
}
