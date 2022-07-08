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

    //ゲーム開始時に呼ばれる
    private void Start()
    {
        //確認パネルを非表示
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
        //Escが押された時
        if (Input.GetKey(KeyCode.Escape))
        {
            ConfirmationPanel.SetActive(true); //確認パネル表示
 
            //パネルアニメーション
            //var sequence = DOTween.Sequence()
            //    .Append(BoxCanvasGroup.DOFade(endValue: 1f, duration: 0.2f))
            //    .Join(BoxRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, y: 2), duration: 1f).SetEase(Ease.OutCubic))
            //    .Join(BlurRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, y: 2), duration: 1f).SetEase(Ease.OutCubic))
            //    .Append(BoxRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, 350.0f), duration: 1f).SetEase(Ease.OutCubic))
            //    .Join(BlurRectTransform.DOSizeDelta(endValue: new Vector2(BoxSizeDelta.x, 350.0f), duration: 1f).SetEase(Ease.OutCubic))
            //    //.Append(text.DOFade(endValue: 1f, duration: 0.05f))
            //    .Append(TMP.DOColor(RollOverTextColor, duration: 0.25f)).Play()
            //    .SetDelay(1.7f);


            FalseCanvasGroup.blocksRaycasts = false;//他UIの誤タッチ防止
        }
    }

    public void ClosePanel()
    {
        ConfirmationPanel.SetActive(false);
        FalseCanvasGroup.blocksRaycasts = true;
        
    }


    //ゲーム終了
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
        Application.Quit();//ゲームプレイ終了
#endif
    }
}