using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class Ranking_Input : MonoBehaviour
{
    [SerializeField]
    private GameObject TMPObj;

    TMP_InputField _inputField;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void GetText()
    {
        //InputFieldのTextコンポーネントを取得

        _inputField = GameObject.Find("InputField_UserName").GetComponent<TMP_InputField>();

        //string型に変換
        string name = _inputField.text;

        //データを送る（タイムはまだ。）
        StartCoroutine(SendData(name, TimeAttackManager.Instance.countTime));
    }

    IEnumerator SendData(string userName, double clearTime)
    {
        WWWForm wwwform = new WWWForm();
        wwwform.AddField("UserName", userName);
        wwwform.AddField("ClearTime", clearTime.ToString());

        switch ((int)TimeAttackManager.Instance.nowStage)
        {
            case 0:
                wwwform.AddField("TableName", "GftWRanking1");
                break;

            case 1:
                wwwform.AddField("TableName", "GftWRanking2");
                break;

            case 2:
                wwwform.AddField("TableName", "GftWRanking3");
                break;

            case 3:
                wwwform.AddField("TableName", "GftWRanking4");
                break;
        }

        UnityWebRequest request = UnityWebRequest.Post("https://gftw.soyoshigure.jp/register_score.php", wwwform);
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
    }

}
