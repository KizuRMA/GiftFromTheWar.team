using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimeTest : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image FillImage;

    void Start()
    {
        base.Start();
        FillImage.fillAmount = 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        FillImage.DOFillAmount(endValue: 1f, duration: 0.25f).SetEase(Ease.OutCubic).Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        FillImage.DOFillAmount(endValue: 0f, duration: 0.25f).SetEase(Ease.OutCubic).Play();
    }

    public void OnClick()
    {
        Debug.Log("âüÇ≥ÇÍÇΩ!");  // ÉçÉOÇèoóÕ
    }
}
