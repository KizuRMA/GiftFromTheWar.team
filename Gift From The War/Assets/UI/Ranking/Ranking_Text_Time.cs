using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class Ranking_Text_Time : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI RankText;
    [SerializeField]
    private GameObject Obj;

    [SerializeField]
    SelectRanking selectRanking;

    private string[] rank_time = new string[10];
    private double[] time = new double[10];
    private int index;
    private int sumIndex;



    // Start is called before the first frame update
    void Start()
    {
        //rank_time[0] = "aiueo";
        StartCoroutine(GetData());
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    StartCoroutine(GetData());
        //}

        index = Obj.transform.GetSiblingIndex();

        for (int i = 0; i < 10; i++)
        {
            //rank_time[i] = time[i].ToString(".0");

            //countTimeから経過時間を算出
            double countMinute;
            double countSecond;
            double _time = time[i];
            countMinute = (int)_time / 60;
            _time = _time % 60;
            countSecond = _time;
            rank_time[i] = (countMinute.ToString("00") + ":" + countSecond.ToString("00.00"));
        }

        RankText.text = rank_time[index];
    }

    public void InVokeGetData()
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

        //UnityWebRequest request = UnityWebRequest.Get("https://gftw.soyoshigure.jp/get_data.php?TableName=GftWRanking2");
        
        yield return request.SendWebRequest();
        var rankingData = RankingData.Deserialize(request.downloadHandler.text);

        //foreach (RankingData i in rankingData)
        //{
        //    Debug.Log($"{i.UserName},{i.ClearTime}");
            
        //    rank_time[0] = i.UserName;
        //}

        for(int i = 0; i < 10; i++)
        {
            time[i] = rankingData[i].ClearTime; 
        }
    }
}
