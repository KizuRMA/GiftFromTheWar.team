
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ArrowAnime : MonoBehaviour
{
    [SerializeField] private CanvasGroup CanvasGroup;

    //�₶�邵���o��������
    public void AppArrow()
    {
        CanvasGroup.DOFade(endValue: 1f, duration: 0.2f).SetDelay(2.0f);
    }

    //�₶�邵������
    public void DeleteArrow()
    {
        CanvasGroup.DOFade(endValue: 0f, duration: 0.2f);
    }
}
