using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloseMessageBox : MonoBehaviour
{
    [SerializeField] private CanvasGroup BoxCanvasGroup;
    [SerializeField] private RectTransform BoxRectTransform;
    [SerializeField] private RectTransform BlurRectTransform;

    private Vector2 BoxSizeDelta;

    public void CloseBoxAnimate()
    {
        Initialize();
        //メッセージボックスが閉じる処理
        var sequence = DOTween.Sequence()
            .Append(BoxRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, y: 5), duration: 1f).SetEase(Ease.OutCubic))
            .Join(BlurRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, y: 5), duration: 1f).SetEase(Ease.OutCubic))
            .Append(BoxCanvasGroup.DOFade(endValue: 0f, duration: 0.2f));

    }

    private void Initialize()
    {
        BoxSizeDelta = BoxRectTransform.sizeDelta;
    }

}