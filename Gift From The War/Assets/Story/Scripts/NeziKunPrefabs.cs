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
        neziKunIns = new GameObject[prefabNum];

        // 指定された位置にネジ君を配置
        for (int i = 0; i < (prefabNum); i++)
        {
            neziKunIns[i] = Instantiate(neziKun, neziKunGroup);
            neziKunIns[i].transform.position = spawnList[i].transform.position;
            neziKunIns[i].name = "neziKun" + i;
        }

        for (int i =1; i < prefabNum; i++)
        {
            neziKunIns[i].SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (ScenarioManager.Instance.talkCount)
        {
            case 1:
                neziKunIns[0].SetActive(false);
                break;
        }
    }

}
