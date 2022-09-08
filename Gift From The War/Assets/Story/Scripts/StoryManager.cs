using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    //オブジェクトやスクリプトをとってくる
    [SerializeField] private Transform camTrans;

    //  レイ判定
    [SerializeField] private float handDis;

    //  レイが衝突しているかどうか
    public bool hitFlg;

    //  衝突したオブジェクトの情報を保持しておく
    private string objName;

    public GameObject neziKun;

    // 話しかける時のアイコン用
    [SerializeField] public GameObject talkIcon;

    Scenario scenario;

    // Start is called before the first frame update
    void Start()
    {
        scenario = GameObject.Find("ScenarioManager").GetComponent<Scenario>();
        talkIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray();

        if(hitFlg==true)
        {
            scenario.OpenCommand();
            hitFlg = false;
            talkIcon.SetActive(false);
        }
    }

    //レイ判定
    void Ray()
    {
        if (scenario.scenarioFlg) return;

        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;

        //  レイ投射
        if (Physics.Raycast(ray, out hit, handDis))
        {
            //  衝突しているオブジェクトの各情報を取得
            objName = hit.collider.gameObject.tag;
            neziKun = hit.collider.gameObject;

            //  衝突しているオブジェクトがネジ君だったら
            if (objName == "Nezi")
            {
                // スペースキーで会話ウィンドウ表示
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    hitFlg = true;
                }

                talkIcon.SetActive(true);
            }
        }
        else
        {
            talkIcon.SetActive(false);
        }
    }
}
