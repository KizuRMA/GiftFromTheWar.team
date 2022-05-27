using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ReturnButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup CanvasGroup;


    // Start is called before the first frame update
    void Start()
    {
    }

    //戻るボタンを出現させる
    public void AppButton()
    {
        CanvasGroup.DOFade(endValue: 1f, duration: 0.2f).SetDelay(3.5f);
        CanvasGroup.blocksRaycasts = true;
    }

    //戻るボタンを消す
    public void DeleteButton()
    {
        CanvasGroup.DOFade(endValue: 0f, duration: 0.2f);
        CanvasGroup.blocksRaycasts = false;
    }

}
