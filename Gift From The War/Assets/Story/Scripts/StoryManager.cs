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

    //  コマンドオブジェクト参照用
    [SerializeField] GameObject image;

    // 話しかける時のアイコン用
    [SerializeField] GameObject talkIcon;

    Scenario scenario;

    // Start is called before the first frame update
    void Start()
    {
        scenario = GameObject.Find("ScenarioManager").GetComponent<Scenario>();
        //  コマンド画面を閉じておく
        image.SetActive(false);
        talkIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray();
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
                // クリックで会話ウィンドウ表示
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
