using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DocumentOpen : MonoBehaviour
{
    [SerializeField] GameObject imageObj;
    [SerializeField] GameObject backGroundObj;
    private bool openFlg { get; set; }

    private Image image;
    private Image backGroundImage;

    // Start is called before the first frame update
    void Start()
    {
        openFlg = false;
        imageObj.SetActive(true);
        backGroundObj.SetActive(true);

        image = imageObj.GetComponent<Image>();
        backGroundImage = backGroundObj.GetComponent<Image>();

        image.rectTransform.anchoredPosition = new Vector3(0,-1200.0f,0);

        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        backGroundImage.color = new Color(backGroundImage.color.r, backGroundImage.color.g, backGroundImage.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (openFlg == true)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }

    public void Open()  //�������J���֐�
    {
        if (openFlg == true) return;

        //���Ԃ��~����
        SystemSetting.Instance.Pause();

        CursorManager.Instance.cursorLock = false;

        //�A�j���[�V�������s

        image.rectTransform.DOLocalMoveY(0f, 1.5f).SetEase(Ease.OutQuart).SetUpdate(true).Play();
        image.DOFade(1f,1.0f).SetEase(Ease.InQuart).SetUpdate(true).Play();
        backGroundImage.DOFade(0.5f,0.25f).SetEase(Ease.InQuart).SetUpdate(true).Play();
        openFlg = true;

        

        //�ŗD��UI�Ƃ��ĕ\������
        SystemSetting.Instance.topPriorityUI = true;
    }

    public void Close() //���������֐�
    {
        if (openFlg == false) return;

        //���Ԃ��ĊJ����
        SystemSetting.Instance.Resume();

        CursorManager.Instance.cursorLock = true;

        //�A�j���[�V�������s
        image.DOFade(0f,0.5f).SetEase(Ease.InQuart).SetUpdate(true).Play();
        image.rectTransform.DOLocalMoveY(-1200.0f, 0.8f).SetEase(Ease.InQuart).SetUpdate(true).Play();
        backGroundImage.DOFade(0f,0.5f).SetEase(Ease.InQuart).SetUpdate(true).OnComplete(() => SystemSetting.Instance.topPriorityUI = false).Play();
        openFlg = false;
    }
}
