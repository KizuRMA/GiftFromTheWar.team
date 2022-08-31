using DG.Tweening;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpOut : MonoBehaviour
{

    [SerializeField] private GameObject PanelObject;
    [SerializeField] private CanvasGroup PanelCanvasGroup;
    [SerializeField] private RectTransform ImageTransform;
    //[SerializeField] public float duration;


    public float upDuration;
    public float outDuration;

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
    //private bool openPanel = false;
    

    //ゲーム開始時に呼ばれる
    private void Start()
    {
        Initialize();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenPanel();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ClosePanel();
        }

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
        //ImageTransform.localScale = new Vector3(ImageSizeDelta.x - 2, ImageSizeDelta.y - 2, ImageSizeDelta.z - 2);
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
        //PanelCanvasGroup.DOFade(endValue: 0f, duration: 0.1f);
        //openPanel = false;

        //アニメーション初期化
        ImageTransform.localScale = new Vector3(0, 0, 0);

        
    }

}