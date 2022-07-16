using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonReactionAnime : UIBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image ReactionImage;
    

   
    protected override void Start()
    {
        base.Start();
        
    }

    //マウスがボタンの上に来たら
    public void OnPointerEnter(PointerEventData eventData)
    {
        ReactionImage.DOFade(endValue: 1f, duration: 0.25f).SetEase(Ease.OutCubic).SetUpdate(true).Play();
        AudioManager.Instance.PlaySE("カーソル移動8", isLoop: false);
    }

    //マウスがボタンにかぶっていなかったら
    public void OnPointerExit(PointerEventData eventData)
    {
        ReactionImage.DOFade(endValue: 0f, duration: 0.25f).SetEase(Ease.OutCubic).SetUpdate(true).Play();
        AudioManager.Instance.StopSE("カーソル移動8");
    }

    //ボタンがクリックされたら
    public void OnPointerClick(PointerEventData eventData)
    {
        ReactionImage.DOFade(endValue: 0f, duration: 0.01f).SetEase(Ease.OutCubic).SetUpdate(true).Play();
        AudioManager.Instance.PlaySE("決定ボタンを押す14", isLoop: false);
    }

    public void AnimReset()
    {
        ReactionImage.color = new Color(ReactionImage.color.r, ReactionImage.color.g, ReactionImage.color.b, 0);
    }

}
