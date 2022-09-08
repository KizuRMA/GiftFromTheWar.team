using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SwitchingUI : MonoBehaviour
{
    public List<CanvasGroup> canvasGroups;
    public float durationTime;
    public float firstDelay;
    private int nowIndex;

    void Start()
    {
        nowIndex = 0;

        foreach (var _canvasGroups in canvasGroups)
        {
            _canvasGroups.gameObject.SetActive(false);
            _canvasGroups.alpha = 0.0f;
            _canvasGroups.blocksRaycasts = false;
        }

    }

    public void AppFirstUI()
    {
        nowIndex = 0;
        //�ŏ���UI��\��������
        canvasGroups[nowIndex].gameObject.SetActive(true);
        canvasGroups[nowIndex].DOFade(endValue: 1f, duration: durationTime).SetEase(Ease.OutCubic).SetDelay(firstDelay).OnComplete(() => OnRayCast()).Play();
    }
    public void NextIndex()
    {
        //����UI�𖳌��ɂ��A����UI��L���A�\��������
        canvasGroups[nowIndex].gameObject.SetActive(false);
        nowIndex += 1;
        canvasGroups[nowIndex].gameObject.SetActive(true);
        canvasGroups[nowIndex].DOFade(endValue: 1f, duration: durationTime).SetEase(Ease.OutCubic).OnComplete(() => OnRayCast()).Play();
    }
    private void OnRayCast()
    {
        canvasGroups[nowIndex].blocksRaycasts = true;
    }

    public void CloseUI()
    {
        //���ݕ\������Ă���UI�����
        canvasGroups[nowIndex].blocksRaycasts = false;//�둀��\�h�̈א�ɐ؂�
        canvasGroups[nowIndex].DOFade(endValue: 0f, duration: durationTime).SetEase(Ease.OutCubic).Play();
        canvasGroups[nowIndex].gameObject.SetActive(false);

    }
    public void NextUI()
    {

        if (canvasGroups.Count > nowIndex + 1)
        {
            canvasGroups[nowIndex].blocksRaycasts = false;//�둀��\�h�̈א�ɐ؂�
            canvasGroups[nowIndex].DOFade(endValue: 0f, duration: durationTime).SetEase(Ease.OutCubic).OnComplete(() => NextIndex()).Play();
        }

    }
}
