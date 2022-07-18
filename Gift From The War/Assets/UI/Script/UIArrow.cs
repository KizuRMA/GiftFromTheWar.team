using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIArrow : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image image;

    private void OnMouseEnter()
    {
        image.rectTransform.DOScale(1.5f, 0.1f).SetEase(Ease.OutQuart).SetUpdate(true).Play();
        AudioManager.Instance.PlaySE("�J�[�\���ړ�8", isLoop: false);
    }

    private void OnMouseExit()
    {
        image.rectTransform.DOScale(1.0f, 0.1f).SetEase(Ease.OutQuart).SetUpdate(true).Play();
        AudioManager.Instance.StopSE("�J�[�\���ړ�8");
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        image.rectTransform.DOScale(1.3f,0.1f).SetEase(Ease.OutQuart).SetUpdate(true).Play();
        AudioManager.Instance.PlaySE("�J�[�\���ړ�8", isLoop: false);
    }

    //�}�E�X���{�^���ɂ��Ԃ��Ă��Ȃ�������
    public void OnPointerExit(PointerEventData eventData)
    {
        image.rectTransform.DOScale(1.0f, 0.1f).SetEase(Ease.OutQuart).SetUpdate(true).Play();
        AudioManager.Instance.StopSE("�J�[�\���ړ�8");
    }
}
