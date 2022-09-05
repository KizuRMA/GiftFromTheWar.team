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

    // Start is called before the first frame update
    void Start()
    {
        neziKunIns = new GameObject[prefabNum];

        // 指定された位置にネジ君を配置
        for (int i = 0; i < prefabNum; i++)
        {
            neziKunIns[i] = Instantiate(neziKun, neziKunGroup);
            neziKunIns[i].transform.position = spawnList[i].transform.position;
            neziKunIns[i].name = "neziKun" + i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 話しかけた回数で表示するネジ君を変更する
        switch (ScenarioManager.Instance.talkCount)
        {
            case 1:
                neziKunIns[1].SetActive(true);
                break;
            case 2:
                neziKunIns[2].SetActive(true);
                break;
            case 3:
                neziKunIns[3].SetActive(true);
                break;
            case 4:
                neziKunIns[4].SetActive(true);
                break;
            default:

                for (int i = 1; i < prefabNum; i++)
                {
                    neziKunIns[i].SetActive(false);
                }

                break;
        }

    }
}
