using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class NeziKunPrefabs : MonoBehaviour
{
    [SerializeField]
    private GameObject neziKun = null;

    // 親オブジェクト
    public Transform neziKunGroup;

    // スポーン地点
    public List<GameObject> spawnList = new List<GameObject>();

    // 生成するプレハブの数
    public int prefabNum;

    //  ネジ君のインスタンス生成
    private GameObject[] neziKunIns;

    //　会話可能な相手
    private GameObject conversationPartner;

    // Start is called before the first frame update
    void Start()
    {
        neziKunIns = new GameObject[ScenarioManager.Instance.talkCount+prefabNum];

        // 指定された位置にネジ君を配置
        for (int i = ScenarioManager.Instance.talkCount; i < (ScenarioManager.Instance.talkCount+prefabNum); i++)
        {
            neziKunIns[i] = Instantiate(neziKun, neziKunGroup);
            neziKunIns[i].transform.position = spawnList[i].transform.position;
            neziKunIns[i].name = "neziKun" + i;
        }

        for (int i = ScenarioManager.Instance.talkCount + 1; i < ScenarioManager.Instance.talkCount + prefabNum; i++)
        {
            neziKunIns[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 話しかけた回数で表示するネジ君を変更する
        switch (ScenarioManager.Instance.talkCount)
        {
            case 1:
                neziKunIns[0].SetActive(false);
                neziKunIns[1].SetActive(true);
                break;
            case 2:
                if (prefabNum < 3) return;
                neziKunIns[1].SetActive(false);
                neziKunIns[2].SetActive(true);
                break;
            case 3:

                break;
            case 4:
                neziKunIns[3].SetActive(false);
                neziKunIns[4].SetActive(true);
                break;
            case 5:
                neziKunIns[4].SetActive(false);
                break;
        }

        if (ScenarioManager.Instance.talkCount == 3 && prefabNum == 3)
        {
            neziKunIns[2].SetActive(false);
        }
    }

}
