
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ArrowAnime : MonoBehaviour
{
    [SerializeField] private CanvasGroup CanvasGroup;

    //やじるしを出現させる
    public void AppArrow()
    {
        CanvasGroup.DOFade(endValue: 1f, duration: 0.2f).SetDelay(2.0f);
    }

    //やじるしを消す
    public void DeleteArrow()
    {
        CanvasGroup.DOFade(endValue: 0f, duration: 0.2f);
    }
}
