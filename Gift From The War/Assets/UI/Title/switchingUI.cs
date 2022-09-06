using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class switchingUI : MonoBehaviour
{
    public List<CanvasGroup> canvasGroups;
    public float durationTime;
    private CanvasGroup nowCanvas;
    private int nowIndex;

    // Start is called before the first frame update

    void Start()
    {
        nowIndex = 0;

        if (canvasGroups.Count >= 1)
        {
            nowCanvas = canvasGroups[nowIndex];
        }


        foreach (var _canvasGroups in canvasGroups)
        {
            _canvasGroups.gameObject.SetActive(false);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextUI()
    {
        //�؂�ւ��A�j���[�V����
        canvasGroups[nowIndex].DOFade(endValue: 0f, duration: durationTime).SetEase(Ease.OutCubic).SetUpdate(true).Play();
        //�p�l���A�j���[�V����
        //var sequence = DOTween.Sequence()
        //    .Append(PanelCanvasGroup.DOFade(endValue: 1f, duration: upDuration))
        //    .Join(ImageTransform.DOScale(endValue: new Vector3(ImageSizeDelta.x, ImageSizeDelta.y, ImageSizeDelta.z), duration: upDuration).SetEase(upEaseType))
        //    .AppendCallback(() => OnRayCast())
        //    .Play();


    }
}
