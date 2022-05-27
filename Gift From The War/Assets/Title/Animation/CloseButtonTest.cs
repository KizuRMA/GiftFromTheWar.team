using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloseButtonTest : MonoBehaviour
{
    [SerializeField] private CanvasGroup ListsCanvasGroup;
    [SerializeField] private CanvasGroup ButtonCanvasGroup;
    [SerializeField] private Image text;
    [SerializeField] private RectTransform ButtonRectTransform;
    [SerializeField] private RectTransform FillRectTransform;
    [SerializeField] private RectTransform BackRectTransform;

    private Vector2 ButtonSizeDelta;
    
    public void ButtonCloseAnimate()
    {
        Initialize();
        //É{É^ÉìÇ™ï¬Ç∂ÇÈèàóù
        var sequence = DOTween.Sequence()
            .Append(text.DOFade(endValue: 0f, duration: 0.05f))
            .Append(ButtonRectTransform.DOSizeDelta(endValue: new Vector2(ButtonSizeDelta.x, y: 5), duration: 1f)).SetEase(Ease.OutCubic).Play()
            .Join(FillRectTransform.DOSizeDelta(endValue: new Vector2(ButtonSizeDelta.x, y: 5), duration: 1f)).SetEase(Ease.OutCubic).Play()
            .Join(BackRectTransform.DOSizeDelta(endValue: new Vector2(ButtonSizeDelta.x, y: 5), duration: 1f)).SetEase(Ease.OutCubic).Play()
            .Append(ButtonCanvasGroup.DOFade(endValue: 0f, duration: 0.2f));

        ListsCanvasGroup.blocksRaycasts = false;
    }
    
    private void Initialize()
    {
        ButtonSizeDelta = ButtonRectTransform.sizeDelta;
        //ButtonRectTransform.sizeDelta = Vector2.zero;
    }

}
