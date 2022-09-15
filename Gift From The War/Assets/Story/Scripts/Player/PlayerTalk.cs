using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTalk : MonoBehaviour
{
    // 会話相手
    private GameObject talkPartner;

    [SerializeField] private GameObject talkIcon;
    [SerializeField] private GameObject talkUI;

    //インスペクターからは触れないようにしておく
    [System.NonSerialized] public bool talkFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        talkIcon.SetActive(false);
        talkUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //会話相手をセットする
    public void SetTalkPartner(GameObject partnerObj)
    {
        talkIcon.SetActive(true);
        talkPartner = partnerObj;
    }

    public void StartTalk()
    {
        talkFlg = true;

        talkUI.SetActive(true);
        talkIcon.SetActive(false);
    }

    //  文章を読み終わったらコマンド画面を終了させる
    public void EndTalk()
    {
        talkUI.SetActive(false);
        talkPartner.SetActive(false);
        talkFlg = false;
        ScenarioManager.Instance.endFlg = true;
    }

    public void OnActiceIcon()
    {
        talkIcon.SetActive(true);
    }

    public void OnActiceUI()
    {
        talkUI.SetActive(true);
    }

    public void OffActiceIcon()
    {
        talkIcon.SetActive(false);
    }
}
