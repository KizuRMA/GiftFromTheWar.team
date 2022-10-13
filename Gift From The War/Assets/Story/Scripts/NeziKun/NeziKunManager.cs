using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class NeziKunManager : MonoBehaviour
{
    // 親オブジェクトのリスト
    [SerializeField] public List<Transform> parentList= new List<Transform>();

    //スポーンリスト
    [SerializeField] private List<NeziKunSpawn> list = null;

    [System.NonSerialized] public int maxCount;

    // Start is called before the first frame update
    void Start()
    {   
          if (ScenarioData.Instance != null&&ScenarioData.Instance.enable)
        {
            maxCount = list.Count;

            ScenarioData.Instance.saveData.maxCount = maxCount;
            //ScenarioData.Instance.WriteFile();

            int index = ScenarioData.Instance.saveData.neziKunCount;




            for (int i = index; i < list.Count; i++)
            {
                //指定した位置にネジ君生成
                GameObject game = Instantiate(list[i].neziKun);
                game.transform.position = list[i].spawn.position;

                //親子関係設定
                game.transform.parent = parentList[i];
                game.name = "NeziKun" + i;
            }


            for (int i = index + 1; i < list.Count; i++)
            {
                list[i].gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       if(ScenarioData.Instance!=null&&ScenarioData.Instance.enable == false)
        {
            enabled = false;
            Debug.Log("False");
        }
    }

    public void Spawn()
    {
        if (ScenarioManager.Instance != null)
        {
            int index = ScenarioManager.Instance.neziKunCount;

            if (index < list.Count)
            {
                list[index].gameObject.SetActive(true);
            }
        }
    }
}
