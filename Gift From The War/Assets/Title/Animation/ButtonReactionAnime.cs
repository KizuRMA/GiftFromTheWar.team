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

    //�}�E�X���{�^���̏�ɗ�����
    public void OnPointerEnter(PointerEventData eventData)
    {
        ReactionImage.DOFade(endValue: 1f, duration: 0.25f).SetEase(Ease.OutCubic).SetUpdate(true).Play();
        AudioManager.Instance.PlaySE("�J�[�\���ړ�8", isLoop: false);
    }

    //�}�E�X���{�^���ɂ��Ԃ��Ă��Ȃ�������
    public void OnPointerExit(PointerEventData eventData)
    {
        ReactionImage.DOFade(endValue: 0f, duration: 0.25f).SetEase(Ease.OutCubic).SetUpdate(true).Play();
        AudioManager.Instance.StopSE("�J�[�\���ړ�8");
    }

    //�{�^�����N���b�N���ꂽ��
    public void OnPointerClick(PointerEventData eventData)
    {
        ReactionImage.DOFade(endValue: 0f, duration: 0.01f).SetEase(Ease.OutCubic).SetUpdate(true).Play();
        AudioManager.Instance.PlaySE("����{�^��������14", isLoop: false);
    }

    public void AnimReset()
    {
        ReactionImage.color = new Color(ReactionImage.color.r, ReactionImage.color.g, ReactionImage.color.b, 0);
    }

}
