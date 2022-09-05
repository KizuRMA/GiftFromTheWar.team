using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RankingDataSystem : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    IEnumerator SendData(string userName, double clearTime)
    {
        WWWForm wwwform = new WWWForm();
        wwwform.AddField("UserName", userName);
        wwwform.AddField("ClearTime", clearTime.ToString());

        UnityWebRequest request = UnityWebRequest.Post("https://gftw.soyoshigure.jp/register_score.php", wwwform);
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
    }

    IEnumerator GetData()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://gftw.soyoshigure.jp/get_data.php?TableName=GftWRanking");
        yield return request.SendWebRequest();
        var rankingData = RankingData.Deserialize(request.downloadHandler.text);

        foreach (RankingData i in rankingData)
        {
            Debug.Log($"{i.UserName},{i.ClearTime}");
        }
    }
}
