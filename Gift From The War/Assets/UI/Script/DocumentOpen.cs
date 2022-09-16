using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DocumentOpen : MonoBehaviour
{
    [SerializeField] private List<Image> images;
    [SerializeField] private Image backGroundImage;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI pageNumberText;
    private Image image;
    private CanvasGroup showCanvas;

    [Header("必要なら設定する項目(設定しなくてもいい)")]
    [SerializeField] private float backGroundAlpha = 0.5f;
    [SerializeField] private float backGroundAlphaSpeed = 0.25f;

    private bool openFlg { get; set; }
    private int imageIndex;

    // Start is called before the first frame update
    void Start()
    {
        openFlg = false;
        imageIndex = 0;
        if (images.Count >= 1)
        {
            image = images[imageIndex];
            showCanvas = images[imageIndex].GetComponent<CanvasGroup>();
        }

        foreach (var _images in images)
        {
            _images.gameObject.SetActive(false);
        }

        image.gameObject.SetActive(true);
        backGroundImage.gameObject.SetActive(true);
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        image.rectTransform.anchoredPosition = new Vector3(0,-1200.0f,0);

        //image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        backGroundImage.color = new Color(backGroundImage.color.r, backGroundImage.color.g, backGroundImage.color.b, 0);
        canvasGroup.alpha = 0;
        showCanvas.alpha = 0;

        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        if (SystemSetting.Instance.pauseType != SystemSetting.e_PauseType.Document) return;

        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Close();
        }
    }

    public void Open()  //資料を開く関数
    {
        if (openFlg == true) return;

        //時間を停止する
        SystemSetting.Instance.Pause(SystemSetting.e_PauseType.Document);

        CursorManager.Instance.cursorLock = false;

        //ボタンを有効にする
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        //アニメーション実行
        image.rectTransform.DOLocalMoveY(0f, backGroundAlphaSpeed + 1.25f).SetEase(Ease.OutQuart).SetUpdate(true).OnComplete(() => canvasGroup.DOFade(1.0f, 0.5f).SetUpdate(true).Play()).Play();
        image.rectTransform.DOLocalMoveY(0f, backGroundAlphaSpeed + 1.25f).SetEase(Ease.OutQuart).SetUpdate(true).Play();
        showCanvas.DOFade(1f, backGroundAlphaSpeed + 0.75f).SetEase(Ease.InQuart).SetUpdate(true).Play();
        backGroundImage.DOFade(backGroundAlpha, 0.25f).SetEase(Ease.InQuart).SetUpdate(true).Play();
        openFlg = true;

        UpdateText();

        //最優先UIとして表示する
        SystemSetting.Instance.topPriorityUI = true;
    }

    public void Close() //資料を閉じる関数
    {
        if (openFlg == false) return;

        //時間を再開する
        SystemSetting.Instance.Resume();

        CursorManager.Instance.cursorLock = true;
        Cursor.visible = false;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        //現在行っているアニメーションを削除
        image.rectTransform.DOKill();
        showCanvas.DOKill();
        backGroundImage.DOKill();

        //アニメーション実行
        showCanvas.DOFade(0f,0.5f).SetEase(Ease.InQuart).SetUpdate(true).Play();
        image.rectTransform.DOLocalMoveY(-1200.0f, 0.8f).SetEase(Ease.InQuart).SetUpdate(true).Play();
        backGroundImage.DOFade(0f,0.5f).SetEase(Ease.InQuart).SetUpdate(true).OnComplete(() => SystemSetting.Instance.topPriorityUI = false).Play();
        canvasGroup.DOFade(0.0f, 0.5f).SetUpdate(true).Play();
        openFlg = false;
    }

    public void NextPage()
    {
        if (imageIndex >= images.Count - 1)
        {
            Close();
            return;
        }
        imageIndex = imageIndex + 1;
        image.gameObject.SetActive(false);
        image = images[imageIndex];
        showCanvas = images[imageIndex].GetComponent<CanvasGroup>();
        image.gameObject.SetActive(true);

        UpdateText();
    }

    public void BackPage()
    {
        if (imageIndex <= 0) return;
        image.gameObject.SetActive(false);
        imageIndex = imageIndex - 1;
        image = images[imageIndex];
        showCanvas = images[imageIndex].GetComponent<CanvasGroup>();
        image.gameObject.SetActive(true);

        UpdateText();
    }

    public void UpdateText()
    {
        pageNumberText.text = (imageIndex + 1) + " / " + images.Count;
    }
}
