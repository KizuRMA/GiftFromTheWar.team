using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloseButtonTest : MonoBehaviour
{
    [SerializeField] private CanvasGroup ButtonCanvasGroup;
    [SerializeField] private RectTransform ButtonRectTransform;
    [SerializeField] private RectTransform FillRectTransform;

    private Vector2 ButtonSizeDelta;
    
    public void Animate()
    {
        Initialize();
        //É{É^ÉìÇ™ï¬Ç∂ÇÈèàóù
        var sequence = DOTween.Sequence()
            //.Append(ButtonCanvasGroup.DOFade(endValue:1f, duration:0.2f))
            .Append(ButtonRectTransform.DOSizeDelta(endValue: new Vector2(ButtonSizeDelta.x, y: 5), duration: 1f)).SetEase(Ease.OutCubic).Play()
            .Join(FillRectTransform.DOSizeDelta(endValue: new Vector2(ButtonSizeDelta.x, y: 5), duration: 1f)).SetEase(Ease.OutCubic).Play();
        //.Append(ButtonRectTransform.DOSizeDelta(endValue: new Vector2(ButtonSizeDelta.x, ButtonSizeDelta.y), duration: 1f));
    }
    
    private void Initialize()
    {
        ButtonSizeDelta = ButtonRectTransform.sizeDelta;
        //ButtonRectTransform.sizeDelta = Vector2.zero;
    }
    
}
