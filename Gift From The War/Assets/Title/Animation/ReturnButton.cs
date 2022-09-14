using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ReturnButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CanvasGroup CanvasGroup;


    // Start is called before the first frame update
    void Start()
    {
    }

    private void RaycastOnOff()
    {
        if(CanvasGroup.blocksRaycasts)
        {
            CanvasGroup.blocksRaycasts = false;
        }
        else
        {
            CanvasGroup.blocksRaycasts = true;
        }
    }

    //�߂�{�^�����o��������
    public void AppButton()
    {
        CanvasGroup.DOFade(endValue: 1f, duration: 0.2f).SetDelay(3.5f);
        Invoke("RaycastOnOff", 4.0f);
    }

    //�߂�{�^��������
    public void DeleteButton()
    {
        RaycastOnOff();
        CanvasGroup.DOFade(endValue: 0f, duration: 0.2f);
    }

    //�{�^���������ꂽ��
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySE("�L�����Z��1", isLoop: false);
    }

}
