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

    //�}�E�X���{�^���̏�ɗ�����
    public void OnPointerEnter(PointerEventData eventData)
    {
        FillImage.DOFillAmount(endValue: 1f, duration: 0.25f).SetEase(Ease.OutCubic).Play();
        emiOnEvent.Invoke();
        AudioManager.Instance.PlaySE("�J�[�\���ړ�8", isLoop: false);
    }

    //�}�E�X���{�^���ɂ��Ԃ��Ă��Ȃ�������
    public void OnPointerExit(PointerEventData eventData)
    {
        FillImage.DOFillAmount(endValue: 0f, duration: 0.25f).SetEase(Ease.OutCubic).Play();
        emiOffEvent.Invoke();
        AudioManager.Instance.StopSE("�J�[�\���ړ�8");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySE("����{�^��������14", isLoop: false);
    }
}
