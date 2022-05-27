using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    [SerializeField] private CanvasGroup BoxCanvasGroup;
    [SerializeField] private CanvasGroup ButtonCanvasGroup;
    [SerializeField] private RectTransform BoxRectTransform;
    [SerializeField] private RectTransform BlurRectTransform;
    //[SerializeField] private Image text;

    private Vector2 BoxSizeDelta;

    public void OpenAnimate()
    {
        Initialize();

        //メッセージボックスが開く処理
        var sequence = DOTween.Sequence()
            .Append(BoxCanvasGroup.DOFade(endValue: 1f, duration: 0.2f))
            .Join(BoxRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, y: 2), duration: 1f).SetEase(Ease.OutCubic))
            .Join(BlurRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, y: 2), duration: 1f).SetEase(Ease.OutCubic))
            .Append(BoxRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, 350.0f), duration: 1f).SetEase(Ease.OutCubic))
            .Join(BlurRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, 350.0f), duration: 1f).SetEase(Ease.OutCubic))
            .Append(ButtonCanvasGroup.DOFade(endValue: 1f, duration: 0.05f))
            //.Append(text.DOFade(endValue: 1f, duration: 0.05f))
            .SetDelay(1.7f);


    }

    public void CloseAnimate()
    {
        
        //メッセージボックスが閉じる処理
        var sequence = DOTween.Sequence()
            //.Append(text.DOFade(endValue: 0f, duration: 0.05f))
            .Append(ButtonCanvasGroup.DOFade(endValue: 0f, duration: 0.05f))
            .Append(BoxRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, y: 5), duration: 1f).SetEase(Ease.OutCubic))
            .Join(BlurRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, y: 5), duration: 1f).SetEase(Ease.OutCubic))
            .Append(BoxCanvasGroup.DOFade(endValue: 0f, duration: 0.2f));

    }

    private void Initialize()
    {
        BoxSizeDelta = BoxRectTransform.sizeDelta;
        BoxRectTransform.sizeDelta = Vector2.zero;
        BlurRectTransform.sizeDelta = Vector2.zero;
    }
}