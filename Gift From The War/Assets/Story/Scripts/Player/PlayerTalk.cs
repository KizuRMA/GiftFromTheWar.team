using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTalk : MonoBehaviour
{
    [SerializeField] private Transform camTrans;
    //  レイ判定
    [SerializeField] private float handDis;

    //  レイが衝突しているかどうか
    public bool hitFlg;

    //  衝突したオブジェクトの情報を保持しておく
    private string tagName;
    private GameObject hitObj;

    // 会話相手
    private GameObject talkPartner;

    [SerializeField] private GameObject talkIcon;
    [SerializeField] private GameObject talkUI;

    private bool talkFlg = false;

    public bool iconFlg=false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray();
        StartTalk();
        //EndTalk();
    }

    private void Ray()
    {
        if (iconFlg) return;

        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;

        //  レイ投射
        if (Physics.Raycast(ray, out hit, handDis))
        {
            //  衝突しているオブジェクトの各情報を取得
            tagName = hit.collider.gameObject.tag;
            hitObj = hit.collider.gameObject;

            //  衝突しているオブジェクトがネジ君だったら
            if (tagName == "Nezi")
            {
                // オブジェクトを会話相手として設定する
                SetTalkPartner(hitObj);
                
                // スペースキーが押されなかったら処理しない
                if (!Input.GetKeyDown(KeyCode.Space)) return;
                talkFlg = true;
            }
        }
        else
        {
            talkIcon.SetActive(false);
        }
    }

    // 会話相手をセットする
    public void SetTalkPartner(GameObject partnerObj)
    {
        talkIcon.SetActive(true);
        talkPartner = partnerObj;
    }

    public void StartTalk()
    {
        if (talkFlg)
        {
            iconFlg = true;

            talkUI.SetActive(true);
            talkFlg = false;
            talkIcon.SetActive(false);
        }
    }

    //  文章を読み終わったらコマンド画面を終了させる
    public void EndTalk()
    {
        //if (!ScenarioManager.Instance.endFlg) return;

        talkUI.SetActive(false);
        iconFlg = false;
    }

}
