using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBlinking : MonoBehaviour
{
    [SerializeField] public Image image;

    [Header("1���[�v�̒���(�b�P��)")]
    [SerializeField]
    [Range(0.1f, 10.0f)]

    float duration = 1.0f;
    public bool IsShow; //
    private bool IsStart;
    private bool IsPressKeyCord;

    Color defaltStartColor;
    Color defaltEndColor;
    float time;

    [Header("���[�v�J�n���̐F")]
    [SerializeField]
    Color startColor = new Color(1, 1, 1, 1);
    //���[�v�I��(�܂�Ԃ�)���̐F��0�`255�܂ł̐����Ŏw��B
    [Header("���[�v�I�����̐F")]
    [SerializeField]
    Color endColor = new Color(1, 1, 1, 0.64f);
    [Header("�o�����鑬��")]
    [SerializeField] public float showSpeed = 5;


    [Header("�K�v�Ȃ�ݒ肷�鍀��(�ݒ肵�Ȃ��Ă�����)")]
    [SerializeField] KeyCode key = KeyCode.None;
    [SerializeField] TextMeshProUGUI explanatoryText = null;
    Color textColor;

    [TooltipAttribute("�����ɕ\���ł��Ȃ�UI"), SerializeField] List<Image> nonTogetherImages = null;


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
        //UI�\�����J�n���Ă��Ȃ��ꍇ
        if (IsStart == false) { PreStart(); return; };

        time += Time.deltaTime;

        if (IsShow == true) //�\����Ԃ̎�
        {
            NonTogetherImagesUpdate();
            KeyUpdate();
            image.color = Color.Lerp(startColor, endColor, Mathf.PingPong(time / duration, 1.0f));
            return;
        }

        //��\����Ԃ̎�

        //���݂̐F���珙�X�ɓ����ɂ���
        image.color = Color.Lerp(startColor, endColor, Mathf.PingPong(time / duration, 1.0f));

        //�摜����\���ɂȂ�Ɠ����Ƀe�L�X�g�������ɂ���
        if(explanatoryText != null)
        {
            Color _startColor = textColor;
            Color _endColor = new Color(textColor.r, textColor.g, textColor.b, 0);
            explanatoryText.color = Color.Lerp(_startColor, _endColor, Mathf.PingPong(time / duration, 1.0f));
        }

        if (image.color.a  <= 0.01f)
        {
            //�摜������
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

        if (Input.GetKeyDown(key) || IsPressKeyCord == true)    //��\���ɂ���ꍇ
        {
            //�X�^�[�g�ƃG���h��ύX����
            NonShow();
            IsShow = false;
            IsPressKeyCord = false;
        }
    }

    public void SetActive() //�摜��\����Ԃɂ���
    {
        if (IsShow == true || IsStart == true || image.enabled == true) return;

        image.enabled = true;
        image.color = new Color(startColor.r, startColor.g, startColor.b, 0);

        //�X�^�[�g�J���[���f�t�H���g�ɖ߂�
        startColor = defaltStartColor;
        endColor = new Color(endColor.r, endColor.g, endColor.b, 0);
        time = 0;
    }

    public void PreStart()  //�摜��\�����鏀��������֐�
    {
        if (image.enabled == false) return;

        time += Time.deltaTime * showSpeed;
        //������Ԃ��珙�X�ɕs�����ɂ��Ă���
        image.color = Color.Lerp(endColor, startColor, Mathf.PingPong(time / duration, 1.0f));

        //�摜����\���ɂȂ�Ɠ����Ƀe�L�X�g�������ɂ���
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
            //�G���h�J���[���f�t�H���g�ɖ߂�
            endColor = defaltEndColor;
            image.color = startColor;

            //�\�����J�n����
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
        //�X�^�[�g�ƃG���h��ύX����
        startColor = image.color;
        endColor = new Color(endColor.r, endColor.g, endColor.b, 0);
        time = 0;
    }
}
