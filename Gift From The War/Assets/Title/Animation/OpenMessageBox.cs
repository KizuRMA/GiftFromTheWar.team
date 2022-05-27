using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenMessageBox : MonoBehaviour
{
    [SerializeField] private CanvasGroup BoxCanvasGroup;
    [SerializeField] private RectTransform BoxRectTransform;
    [SerializeField] private RectTransform BlurRectTransform;
    [SerializeField] private Image text;

    private Vector2 BoxSizeDelta;

    public void OpenBoxAnimate()
    {
        Initialize();

        //メッセージボックスが開く処理
        var sequence = DOTween.Sequence()
            .Append(BoxCanvasGroup.DOFade(endValue: 1f, duration: 0.2f))
            .Join(BoxRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, y: 2), duration: 1f).SetEase(Ease.OutCubic))
            .Join(BlurRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, y: 2), duration: 1f).SetEase(Ease.OutCubic))
            .Append(BoxRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, 350.0f), duration: 1f).SetEase(Ease.OutCubic))
            .Join(BlurRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, 350.0f), duration: 1f).SetEase(Ease.OutCubic))
            .Append(text.DOFade(endValue: 1f, duration: 0.05f))
            .SetDelay(1.7f);


    }

    private void Initialize()
    {
        BoxSizeDelta = BoxRectTransform.sizeDelta;
        BoxRectTransform.sizeDelta = Vector2.zero;
        BlurRectTransform.sizeDelta = Vector2.zero;
    }
}
