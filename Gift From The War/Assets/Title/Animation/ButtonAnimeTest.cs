using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonAnimeTest : UIBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image FillImage;
    //[SerializeField] private TextMeshProUGUI TMP;
    [SerializeField] private Color RollOverTextColor;
    //private Color BaseTextColor;
    [SerializeField] private UnityEvent emiOnEvent = new UnityEvent();
    [SerializeField] private UnityEvent emiOffEvent = new UnityEvent();

    protected override void Start()
    {
        base.Start();
        FillImage.fillAmount = 0;
        //BaseTextColor = TMP.color;
    }

    //マウスがボタンの上に来たら
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySE("カーソル移動8", isLoop: false);
        FillImage.DOFillAmount(endValue: 1f, duration: 0.25f).SetEase(Ease.OutCubic).Play();
        emiOnEvent.Invoke();
    }

    //マウスがボタンにかぶっていなかったら
    public void OnPointerExit(PointerEventData eventData)
    {
        AudioManager.Instance.StopSE("カーソル移動8");
        FillImage.DOFillAmount(endValue: 0f, duration: 0.25f).SetEase(Ease.OutCubic).Play();
        emiOffEvent.Invoke();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySE("決定ボタンを押す14", isLoop: false);
    }
}
