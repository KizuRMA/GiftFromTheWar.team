using DG.Tweening;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpOut : MonoBehaviour
{

    public GameObject PanelObject;
    public CanvasGroup PanelCanvasGroup;
    public RectTransform ImageTransform;
    public float upDuration;
    public float outDuration;
    public bool forceOpen = false;

    public enum EaseType
    {
        Linear, InCubic, OutCubic, InOutCubic, OutQuad, OutBounce
    }

    //���݂̎��
    public EaseType PopUpEaseType;
    public EaseType PopOutEaseType;

    private Vector3 ImageSizeDelta;
    private Ease upEaseType;
    private Ease outEaseType;
    

    //�Q�[���J�n���ɌĂ΂��
    private void Start()
    {
        Initialize();

        //�V�[���J�ڎ������I�Ƀ|�b�v�A�b�v���邩�ǂ���
        if(forceOpen)
        {
            OpenPanel();
        }
    }

    void Update()
    {
        //�f�o�b�O�p
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenPanel();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ClosePanel();
        }

        //�I�������C�[�W���O��ݒ肷��
        switch (PopUpEaseType)
        {
            case EaseType.Linear:
                upEaseType = Ease.Linear;
                break;
            case EaseType.InCubic:
                upEaseType = Ease.InCubic;
                break;
            case EaseType.OutCubic:
                upEaseType = Ease.OutCubic;
                break;
            case EaseType.InOutCubic:
                upEaseType = Ease.InOutCubic;
                break;
            case EaseType.OutQuad:
                upEaseType = Ease.OutQuad;
                break;
            case EaseType.OutBounce:
                upEaseType = Ease.OutBounce;
                break;
        }

        switch (PopOutEaseType)
        {
            case EaseType.Linear:
                outEaseType = Ease.Linear;
                break;
            case EaseType.InCubic:
                outEaseType = Ease.InCubic;
                break;
            case EaseType.OutCubic:
                outEaseType = Ease.OutCubic;
                break;
            case EaseType.InOutCubic:
                outEaseType = Ease.InOutCubic;
                break;
            case EaseType.OutQuad:
                outEaseType = Ease.OutQuad;
                break;
            case EaseType.OutBounce:
                outEaseType = Ease.OutBounce;
                break;
        }


    }

    public void OpenPanel()
    {

        PanelObject.SetActive(true); //�m�F�p�l���\��

        //�p�l���A�j���[�V����
        var sequence = DOTween.Sequence()
            .Append(PanelCanvasGroup.DOFade(endValue: 1f, duration: upDuration))
            .Join(ImageTransform.DOScale(endValue: new Vector3(ImageSizeDelta.x, ImageSizeDelta.y, ImageSizeDelta.z), duration: upDuration).SetEase(upEaseType))
            .AppendCallback(() => OnRayCast())
            .Play();

    }

    //�p�l�������
    public void ClosePanel()
    {
        //�p�l����G��Ȃ��悤�ɂ���
        PanelCanvasGroup.blocksRaycasts = false;

        //�p�l���A�j���[�V����
        var sequence = DOTween.Sequence()
            .Append(PanelCanvasGroup.DOFade(endValue: 0f, duration: outDuration))
            .Join(ImageTransform.DOScale(endValue: new Vector3(0, 0, 0), duration: outDuration).SetEase(outEaseType))
            .AppendCallback(() => OffRayCast())
            .Play();

    }

    private void Initialize()
    {
        //�p�l�����\��
        PanelObject.SetActive(false);
        //��^�b�`�h�~
        PanelCanvasGroup.blocksRaycasts = false;

        //�A�j���[�V����������X�P�[���l��ݒ�
        ImageSizeDelta = ImageTransform.localScale;
        ImageTransform.localScale = new Vector3(0, 0, 0);

    }

    private void OnRayCast()
    {
        PanelCanvasGroup.blocksRaycasts = true;
    }

    private void OffRayCast()
    {
        //�p�l�����\���A������
        PanelObject.SetActive(false);

        //�A�j���[�V����������
        ImageTransform.localScale = new Vector3(0, 0, 0);

    }

}