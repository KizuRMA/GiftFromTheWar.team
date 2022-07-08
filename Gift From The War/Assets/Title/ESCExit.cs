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

    //�Q�[���J�n���ɌĂ΂��
    private void Start()
    {
        //�m�F�p�l�����\��
        ConfirmationPanel.SetActive(false);
        //PanelCanvasGroup.alpha = 0;
        //PanelCanvasGroup.blocksRaycasts = false;
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
            ConfirmationPanel.SetActive(true); //�m�F�p�l���\��
 
            //�p�l���A�j���[�V����
            //var sequence = DOTween.Sequence()
            //    .Append(BoxCanvasGroup.DOFade(endValue: 1f, duration: 0.2f))
            //    .Join(BoxRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, y: 2), duration: 1f).SetEase(Ease.OutCubic))
            //    .Join(BlurRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, y: 2), duration: 1f).SetEase(Ease.OutCubic))
            //    .Append(BoxRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, 350.0f), duration: 1f).SetEase(Ease.OutCubic))
            //    .Join(BlurRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, 350.0f), duration: 1f).SetEase(Ease.OutCubic))
            //    //.Append(text.DOFade(endValue: 1f, duration: 0.05f))
            //    .Append(TMP.DOColor(RollOverTextColor, duration: 0.25f)).Play()
            //    .SetDelay(1.7f);


            FalseCanvasGroup.blocksRaycasts = false;//��UI�̌�^�b�`�h�~
        }
    }

    public void ClosePanel()
    {
        ConfirmationPanel.SetActive(false);
        FalseCanvasGroup.blocksRaycasts = true;
        
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