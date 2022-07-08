using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    //フェードアウト処理の開始、完了を管理するフラグ
    public bool isFadeOut { get; set; }
    //フェードイン処理の開始、完了を管理するフラグ
    private bool isFadeIn = true;
    //透明度が変わるスピード
    [SerializeField] private float fadeSpeed;
    //画面をフェードさせるための画像をパブリックで取得
    [SerializeField] private Image fadeImage;
    float red, green, blue, alfa;
    //シーン遷移のための型
    string afterScene;

    void Start()
    {
        Debug.Log(Screen.width);
        Screen.SetResolution(Screen.width, Screen.height, true);
        DontDestroyOnLoad(this);
        SetRGBA(0, 0, 0, 1);
        isFadeOut = false;
        fadeImage.enabled = false;
    }

    //シーン遷移が完了した際にフェードインを開始するように設定
    public void fadeInStart(int red, int green, int blue, int alfa)
    {
        fadeImage.enabled = true;
        SetRGBA(red, green, blue, alfa);
        SetColor();
        isFadeIn = true;
    }

    public void fadeOutStart(int red, int green, int blue, int alfa)
    {
        fadeImage.enabled = true;
        SetRGBA(red, green, blue, alfa);
        SetColor();
        isFadeOut = true;
    }

    void Update()
    {
        if (isFadeIn)
        {
            //不透明度を徐々に下げる
            alfa -= fadeSpeed * Time.deltaTime;
            //変更した透明度を画像に反映させる関数を呼ぶ
            SetColor();
            if (alfa <= 0)
            {
                isFadeIn = false;
                fadeImage.enabled = false;
            }
        }
        if (isFadeOut)
        {
            //不透明度を徐々に上げる
            alfa += fadeSpeed * Time.deltaTime;
            //変更した透明度を画像に反映させる関数を呼ぶ
            SetColor();
            if (alfa >= 1)
            {
                isFadeOut = false;
            }
        }
    }

    //画像に色を代入する関数
    void SetColor()
    {
        fadeImage.color = new Color(red, green, blue, alfa);
    }

    //色の値を設定するための関数
    public void SetRGBA(int r, int g, int b, int a)
    {
        red = r;
        green = g;
        blue = b;
        alfa = a;
    }
}
