using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TargetSetting : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image hitImage;
    [SerializeField] private hand Hand;

    void Start()
    {
        image.enabled = false;
        hitImage.color = new Color(hitImage.color.r, hitImage.color.g, hitImage.color.b, 0);
    }

    void Update()
    {
        if(SaveManager.Instance.nowSaveData.getGunFlg)
        {
            image.enabled = true;
        }

        if(Hand.GetHandFlg() || SystemSetting.Instance.topPriorityUI == true)
        {
            image.enabled = false;
        }
    }

    public void HitAnime()
    {
        hitImage.DOFade(1f, 0.1f).OnComplete(() => hitImage.DOFade(0f, 0.5f).SetEase(Ease.InQuart).SetUpdate(true).Play()).SetEase(Ease.InQuart).SetUpdate(true).Play();
    }
}
