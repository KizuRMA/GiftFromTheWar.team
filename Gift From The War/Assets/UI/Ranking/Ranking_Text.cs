using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class Ranking_Text : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI RankText;
    [SerializeField]
    private GameObject Obj;

    [SerializeField]
    SelectRanking selectRanking;


    private string[] rank_name = new string[10];
    private int index;
    private int RankingType;

    void Start()
    {
        StartCoroutine(GetData());
    }

    void Update()
    {
        index = Obj.transform.GetSiblingIndex();

        RankText.text = rank_name[index];
    }

    void OnEnable()
    {
        StartCoroutine(GetData());
    }

    public IEnumerator GetData()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://gftw.soyoshigure.jp/get_data.php?TableName=GftWRanking1");

        switch (selectRanking.rankingType)
        {
            case 0:
                request = UnityWebRequest.Get("https://gftw.soyoshigure.jp/get_data.php?TableName=GftWRanking1");               
                break;

            case 1:
                request = UnityWebRequest.Get("https://gftw.soyoshigure.jp/get_data.php?TableName=GftWRanking2");
                break;

            case 2:
                request = UnityWebRequest.Get("https://gftw.soyoshigure.jp/get_data.php?TableName=GftWRanking3");
                break;

            case 3:
                request = UnityWebRequest.Get("https://gftw.soyoshigure.jp/get_data.php?TableName=GftWRanking4");
                break;

        }

        yield return request.SendWebRequest();
        var rankingData = RankingData.Deserialize(request.downloadHandler.text);

        for (int i = 0; i < 10; i++)
        {
            rank_name[i] = rankingData[i].UserName;
        }
    }
}
