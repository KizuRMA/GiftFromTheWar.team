using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAwake : MonoBehaviour
{
    [SerializeField] private GameObject eye;    //目の画像
    [SerializeField] private GameObject eye2;   //目の画像
    [SerializeField] private float moveEyeSpeed;//瞼の動く速さ
    private RectTransform eyeRec;               //目の画像情報
    private RectTransform eye2Rec;              //目の画像情報
    private bool awakeFinishFlg = false;        //目を覚まし終わったら

    void Start()
    {
        if (eye == null) return;
        eyeRec = eye.GetComponent<RectTransform>();
        eye2Rec = eye2.GetComponent<RectTransform>();
        eyeRec.localPosition = new Vector3(0, 0, 0);
        eye2Rec.localPosition = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (awakeFinishFlg || eyeRec == null) return;

        //瞼を動かす
        eyeRec.localPosition += new Vector3(0, moveEyeSpeed * Time.deltaTime, 0);
        eye2Rec.localPosition += new Vector3(0, -moveEyeSpeed * Time.deltaTime, 0);

        //目が開いたら、画像を非表示
        if (eyeRec.localPosition.y > 720 && eye2Rec.localPosition.y < -720)
        {
            eye.SetActive(false);
            eye2.SetActive(false);

            awakeFinishFlg = true;
        }
    }
}
