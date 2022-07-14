using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonReactionAnime : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image ReactionImage;

    protected override void Start()
    {
        base.Start();

    }

    //マウスがボタンの上に来たら
    public void OnPointerEnter(PointerEventData eventData)
    {
        ReactionImage.DOFade(endValue: 1f, duration: 0.25f).SetEase(Ease.OutCubic).Play();

    }

    //マウスがボタンにかぶっていなかったら
    public void OnPointerExit(PointerEventData eventData)
    {
        ReactionImage.DOFade(endValue: 0f, duration: 0.25f).SetEase(Ease.OutCubic).Play();
    }


}
