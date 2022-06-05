using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloseMessageBox : MonoBehaviour
{
    [SerializeField] private CanvasGroup BoxCanvasGroup;
    [SerializeField] private RectTransform BoxRectTransform;
    [SerializeField] private RectTransform BlurRectTransform;
    //[SerializeField] private Image text;
    [SerializeField] private TextMeshProUGUI TMP;
    [SerializeField] private Color RollOverTextColor;

    private Color BaseTextColor;

    private Vector2 BoxSizeDelta;

    public void CloseBoxAnimate()
    {
        Initialize();
        //メッセージボックスが閉じる処理
        var sequence = DOTween.Sequence()
            .Append(TMP.DOColor(RollOverTextColor, duration: 0.25f)).Play()
            //.Append(text.DOFade(endValue: 0f, duration: 0.05f))
            .Append(BoxRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, y: 5), duration: 1f).SetEase(Ease.OutCubic))
            .Join(BlurRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, y: 5), duration: 1f).SetEase(Ease.OutCubic))
            .Append(BoxCanvasGroup.DOFade(endValue: 0f, duration: 0.2f));

    }

    private void Initialize()
    {
        BaseTextColor = Color.white;
        BoxSizeDelta = BoxRectTransform.sizeDelta;
    }

}