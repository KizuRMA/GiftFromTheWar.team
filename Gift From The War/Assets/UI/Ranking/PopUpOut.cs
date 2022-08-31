using DG.Tweening;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpOut : MonoBehaviour
{

    public GameObject PanelObject;
    public CanvasGroup PanelCanvasGroup;
    public RectTransform ImageTransform;
    public float upDuration;
    public float outDuration;
    public bool forceOpen = false;

    public enum EaseType
    {
        Linear, InCubic, OutCubic, InOutCubic, OutQuad, OutBounce
    }

    //現在の種類
    public EaseType PopUpEaseType;
    public EaseType PopOutEaseType;

    private Vector3 ImageSizeDelta;
    private Ease upEaseType;
    private Ease outEaseType;
    

    //ゲーム開始時に呼ばれる
    private void Start()
    {
        Initialize();

        //シーン遷移時強制的にポップアップするかどうか
        if(forceOpen)
        {
            OpenPanel();
        }
    }

    void Update()
    {
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenPanel();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ClosePanel();
        }

        //選択したイージングを設定する
        switch (PopUpEaseType)
        {
            case EaseType.Linear:
                upEaseType = Ease.Linear;
                break;
            case EaseType.InCubic:
                upEaseType = Ease.InCubic;
                break;
            case EaseType.OutCubic:
                upEaseType = Ease.OutCubic;
                break;
            case EaseType.InOutCubic:
                upEaseType = Ease.InOutCubic;
                break;
            case EaseType.OutQuad:
                upEaseType = Ease.OutQuad;
                break;
            case EaseType.OutBounce:
                upEaseType = Ease.OutBounce;
                break;
        }

        switch (PopOutEaseType)
        {
            case EaseType.Linear:
                outEaseType = Ease.Linear;
                break;
            case EaseType.InCubic:
                outEaseType = Ease.InCubic;
                break;
            case EaseType.OutCubic:
                outEaseType = Ease.OutCubic;
                break;
            case EaseType.InOutCubic:
                outEaseType = Ease.InOutCubic;
                break;
            case EaseType.OutQuad:
                outEaseType = Ease.OutQuad;
                break;
            case EaseType.OutBounce:
                outEaseType = Ease.OutBounce;
                break;
        }


    }

    public void OpenPanel()
    {

        PanelObject.SetActive(true); //確認パネル表示

        //パネルアニメーション
        var sequence = DOTween.Sequence()
            .Append(PanelCanvasGroup.DOFade(endValue: 1f, duration: upDuration))
            .Join(ImageTransform.DOScale(endValue: new Vector3(ImageSizeDelta.x, ImageSizeDelta.y, ImageSizeDelta.z), duration: upDuration).SetEase(upEaseType))
            .AppendCallback(() => OnRayCast())
            .Play();

    }

    //パネルを閉じる
    public void ClosePanel()
    {
        //パネルを触れないようにする
        PanelCanvasGroup.blocksRaycasts = false;

        //パネルアニメーション
        var sequence = DOTween.Sequence()
            .Append(PanelCanvasGroup.DOFade(endValue: 0f, duration: outDuration))
            .Join(ImageTransform.DOScale(endValue: new Vector3(0, 0, 0), duration: outDuration).SetEase(outEaseType))
            .AppendCallback(() => OffRayCast())
            .Play();

    }

    private void Initialize()
    {
        //パネルを非表示
        PanelObject.SetActive(false);
        //誤タッチ防止
        PanelCanvasGroup.blocksRaycasts = false;

        //アニメーションをするスケール値を設定
        ImageSizeDelta = ImageTransform.localScale;
        ImageTransform.localScale = new Vector3(0, 0, 0);

    }

    private void OnRayCast()
    {
        PanelCanvasGroup.blocksRaycasts = true;
    }

    private void OffRayCast()
    {
        //パネルを非表示、無効化
        PanelObject.SetActive(false);

        //アニメーション初期化
        ImageTransform.localScale = new Vector3(0, 0, 0);

    }

}