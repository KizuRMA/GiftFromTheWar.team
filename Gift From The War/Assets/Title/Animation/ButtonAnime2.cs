using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonAnime2 : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image FillImage;
    [SerializeField] private TextMeshProUGUI TMP;
    [SerializeField] private Color RollOverTextColor;
    private Color BaseTextColor;
    [SerializeField] private UnityEvent emiOnEvent = new UnityEvent();
    [SerializeField] private UnityEvent emiOffEvent = new UnityEvent();

    protected override void Start()
    {
        base.Start();
        //FillImage.fillAmount = 0;
        FillImage.rectTransform.sizeDelta = Vector2.zero;
        BaseTextColor = TMP.color;
    }

    //マウスがボタンの上に来たら
    public void OnPointerEnter(PointerEventData eventData)
    {
        //FillImage.DOFillAmount(endValue: 1f, duration: 0.25f).SetEase(Ease.OutCubic).Play();
        FillImage.rectTransform.DOSizeDelta(endValue: Vector2.one * 500, duration: 0.25f)
            .SetEase(Ease.OutCubic)
            .SetLink(gameObject)
            .Play();

        emiOnEvent.Invoke();
        TMP.DOColor(RollOverTextColor, duration: 0.25f).Play();
    }

    //マウスがボタンにかぶっていなかったら
    public void OnPointerExit(PointerEventData eventData)
    {
        //FillImage.DOFillAmount(endValue: 0f, duration: 0.25f).SetEase(Ease.OutCubic).Play();
        FillImage.rectTransform.DOSizeDelta(endValue: Vector2.zero, duration: 0.25f)
            .SetEase(Ease.OutCubic)
            .SetLink(gameObject)
            .Play();
        emiOffEvent.Invoke();
        TMP.DOColor(BaseTextColor, duration: 0.25f).Play();
    }

}
