using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBlinking : MonoBehaviour
{
    [SerializeField] public Image image;

    [Header("1ループの長さ(秒単位)")]
    [SerializeField]
    [Range(0.1f, 10.0f)]

    float duration = 1.0f;
    public bool IsShow; //
    private bool IsStart;
    private bool IsPressKeyCord;

    Color defaltStartColor;
    Color defaltEndColor;
    float time;

    [Header("ループ開始時の色")]
    [SerializeField]
    Color startColor = new Color(1, 1, 1, 1);
    //ループ終了(折り返し)時の色を0～255までの整数で指定。
    [Header("ループ終了時の色")]
    [SerializeField]
    Color endColor = new Color(1, 1, 1, 0.64f);
    [Header("出現する速さ")]
    [SerializeField] public float showSpeed = 5;


    [Header("必要なら設定する項目(設定しなくてもいい)")]
    [SerializeField] KeyCode key = KeyCode.None;
    [SerializeField] TextMeshProUGUI explanatoryText = null;
    Color textColor;

    [TooltipAttribute("同時に表示できないUI"), SerializeField] List<Image> nonTogetherImages = null;


    void Awake()
    {
        if (image == null)
            image = GetComponent<Image>();
    }

    private void Start()
    {
        image.color = new Color(0, 0, 0, 0);
        image.enabled = false;
        defaltStartColor = startColor;
        defaltEndColor = endColor;
        IsStart = false;
        IsShow = false;
        IsPressKeyCord = false;


        time = 0;

        if (explanatoryText != null)
        {
            textColor = explanatoryText.color;
            explanatoryText.color = new Color(0, 0, 0, 0);
        }
    }

    private void Update()
    {
        //UI表示を開始していない場合
        if (IsStart == false) { PreStart(); return; };

        time += Time.deltaTime;

        if (IsShow == true) //表示状態の時
        {
            NonTogetherImagesUpdate();
            KeyUpdate();
            image.color = Color.Lerp(startColor, endColor, Mathf.PingPong(time / duration, 1.0f));
            return;
        }

        //非表示状態の時

        //現在の色から徐々に透明にする
        image.color = Color.Lerp(startColor, endColor, Mathf.PingPong(time / duration, 1.0f));

        //画像が非表示になると同時にテキストも透明にする
        if(explanatoryText != null)
        {
            Color _startColor = textColor;
            Color _endColor = new Color(textColor.r, textColor.g, textColor.b, 0);
            explanatoryText.color = Color.Lerp(_startColor, _endColor, Mathf.PingPong(time / duration, 1.0f));
        }

        if (image.color.a  <= 0.01f)
        {
            //画像を消す
            image.enabled = false;
            IsStart = false;
            image.color = new Color(0, 0, 0, 0);

            if (explanatoryText != null)
            {
                explanatoryText.color = new Color(0, 0, 0, 0);
            }
        }
    }

    private void KeyUpdate()
    {
        if (key == KeyCode.None || IsShow == false) return;

        if (Input.GetKeyDown(key) || IsPressKeyCord == true)    //非表示にする場合
        {
            //スタートとエンドを変更する
            NonShow();
            IsShow = false;
            IsPressKeyCord = false;
        }
    }

    public void SetActive() //画像を表示状態にする
    {
        if (IsShow == true || IsStart == true || image.enabled == true) return;

        image.enabled = true;
        image.color = new Color(startColor.r, startColor.g, startColor.b, 0);

        //スタートカラーをデフォルトに戻す
        startColor = defaltStartColor;
        endColor = new Color(endColor.r, endColor.g, endColor.b, 0);
        time = 0;
    }

    public void PreStart()  //画像を表示する準備をする関数
    {
        if (image.enabled == false) return;

        time += Time.deltaTime * showSpeed;
        //透明状態から徐々に不透明にしていく
        image.color = Color.Lerp(endColor, startColor, Mathf.PingPong(time / duration, 1.0f));

        //画像が非表示になると同時にテキストも透明にする
        if (explanatoryText != null)
        {
            Color _startColor = textColor;
            Color _endColor = new Color(textColor.r, textColor.g, textColor.b, 0);
            explanatoryText.color = Color.Lerp(_endColor, _startColor, Mathf.PingPong(time / duration, 1.0f));
        }

        //
        if (key != KeyCode.None && Input.GetKeyDown(key) && IsPressKeyCord == false)
        {
            IsPressKeyCord = true;
        }

        if (time >= 0.9f)
        {
            //エンドカラーをデフォルトに戻す
            endColor = defaltEndColor;
            image.color = startColor;

            //表示を開始する
            IsStart = true;
            IsShow = true;
            time = 0;

            if (explanatoryText != null)
            {
                explanatoryText.color = textColor;
            }
        }
    }

    private void NonTogetherImagesUpdate()
    {
        if (nonTogetherImages != null)
        {
            foreach (var image in nonTogetherImages)
            {
                if (image.enabled == true)
                {
                    IsShow = false;
                    NonShow();
                }
            }
        }
    }

    private void NonShow()
    {
        //スタートとエンドを変更する
        startColor = image.color;
        endColor = new Color(endColor.r, endColor.g, endColor.b, 0);
        time = 0;
    }
}
