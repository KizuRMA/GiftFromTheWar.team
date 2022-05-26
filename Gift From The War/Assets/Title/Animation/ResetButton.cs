using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResetButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup ListsCanvasGroup;
    [SerializeField] private CanvasGroup ButtonCanvasGroup;
    [SerializeField] private RectTransform ButtonRectTransform;
    [SerializeField] private RectTransform FillRectTransform;
    [SerializeField] private RectTransform BackRectTransform;

    private Vector2 ButtonSizeDelta;

    public void ButtonOpenAnimate()
    {
        Initialize();

        //É{É^ÉìÇ™äJÇ≠èàóù
        var sequence = DOTween.Sequence()
            .Append(ButtonCanvasGroup.DOFade(endValue: 1f, duration: 0.2f))
            .Join(ButtonRectTransform.DOSizeDelta(endValue: new Vector2(ButtonSizeDelta.x, 455f), duration: 1f).SetEase(Ease.OutCubic))
            .Join(FillRectTransform.DOSizeDelta(endValue: new Vector2(ButtonSizeDelta.x, 455f), duration: 1f).SetEase(Ease.OutCubic))
            .Join(BackRectTransform.DOSizeDelta(endValue: new Vector2(ButtonSizeDelta.x, 455f), duration: 1f).SetEase(Ease.OutCubic))
            .SetDelay(2.5f);

        ListsCanvasGroup.blocksRaycasts = true;

    }

    private void Initialize()
    {
        ButtonSizeDelta = ButtonRectTransform.sizeDelta;
        //ButtonRectTransform.sizeDelta = Vector2.zero;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            ButtonOpenAnimate();
        }
    }
}
