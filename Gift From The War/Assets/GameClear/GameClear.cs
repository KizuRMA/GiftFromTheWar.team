using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    [SerializeField] private CanvasGroup CanvasGroup;

    //連続クリック防止
    bool isCalledOnce = false;

    private void Start()
    {
        Initialize();
        TextApp();
    }

    private void Update()
    {
        //何かしらボタンを押したらセレクト画面に移動
        if (Input.anyKey && !isCalledOnce)
        {
            isCalledOnce = true;
            //StartCoroutine(nextScene());
        }
    }

    //テキスト等がフェードインするアニメーション
    public void TextApp()
    {
        CanvasGroup.DOFade(endValue: 1f, duration: 0.2f);

    }

    //後は任せた。

    //private IEnumerator nextScene()
    //{
    //    FadeManager.Instance.fadeOutStart(0, 0, 0, 0);

    //    var asyncOperation = SceneManager.LoadSceneAsync("Scenes/TitleScene");

    //    asyncOperation.allowSceneActivation = false;

    //    while (FadeManager.Instance.isFadeOut)
    //    {
    //        yield return null;
    //    }

    //    asyncOperation.allowSceneActivation = true;

    //    FadeManager.Instance.fadeInStart(0, 0, 0, 1);
    //}

    private void Initialize()
    {
        CanvasGroup.alpha = 0.0f;
    }

}
