using DG.Tweening;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESCExit : MonoBehaviour
{

    [SerializeField] private GameObject ConfirmationPanel;
    [SerializeField] private CanvasGroup FalseCanvasGroup;
    [SerializeField] private CanvasGroup PanelCanvasGroup;
    [SerializeField] private RectTransform ImageTransform;

    private Vector3 ImageSizeDelta3;

    //�Q�[���J�n���ɌĂ΂��
    private void Start()
    {
        Initialize();
    }

    void Update()
    {
        Confirmation();
    }

    public void Confirmation()
    {
        //Esc�������ꂽ��
        if (Input.GetKey(KeyCode.Escape))
        {
            FalseCanvasGroup.blocksRaycasts = false;//��UI�̌�^�b�`�h�~
            
            ConfirmationPanel.SetActive(true); //�m�F�p�l���\��

            //�p�l���A�j���[�V����
            var sequence = DOTween.Sequence()
                .Append(PanelCanvasGroup.DOFade(endValue: 1f, duration: 0.5f))
                .Join(ImageTransform.DOScale(endValue: new Vector3(ImageSizeDelta3.x, ImageSizeDelta3.y,ImageSizeDelta3.z), duration: 0.5f).SetEase(Ease.OutCubic))
                .AppendCallback(() => OnRayCast())
                .Play();

        }
    }

    //�p�l�������
    public void ClosePanel()
    {
        ConfirmationPanel.SetActive(false);
        PanelCanvasGroup.DOFade(endValue: 0f, duration: 0.2f);
        ImageTransform.localScale = new Vector3(ImageSizeDelta3.x - 2, ImageSizeDelta3.y - 2, ImageSizeDelta3.z - 2);
        PanelCanvasGroup.blocksRaycasts = false;
        FalseCanvasGroup.blocksRaycasts = true;

    }

    private void Initialize()
    {
        //�m�F�p�l�����\��
        ConfirmationPanel.SetActive(false);
        PanelCanvasGroup.blocksRaycasts = false;
        ImageSizeDelta3 = ImageTransform.localScale;
        ImageTransform.localScale = new Vector3(ImageSizeDelta3.x - 2, ImageSizeDelta3.y -2, ImageSizeDelta3.z -2);
    }

    private void OnRayCast()
    {
        PanelCanvasGroup.blocksRaycasts = true;
    }

    //�Q�[���I��
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
        Application.Quit();//�Q�[���v���C�I��
#endif
    }
}