using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    //�t�F�[�h�A�E�g�����̊J�n�A�������Ǘ�����t���O
    public bool isFadeOut { get; set; }
    //�t�F�[�h�C�������̊J�n�A�������Ǘ�����t���O
    private bool isFadeIn = true;
    //�����x���ς��X�s�[�h
    [SerializeField] private float fadeSpeed;
    //��ʂ��t�F�[�h�����邽�߂̉摜���p�u���b�N�Ŏ擾
    [SerializeField] private Image fadeImage;
    float red, green, blue, alfa;
    //�V�[���J�ڂ̂��߂̌^
    string afterScene;

    void Start()
    {
        Debug.Log(Screen.width);
        Screen.SetResolution(Screen.width, Screen.height, true);
        DontDestroyOnLoad(this);
        SetRGBA(0, 0, 0, 1);
        isFadeOut = false;
        fadeImage.enabled = false;
    }

    //�V�[���J�ڂ����������ۂɃt�F�[�h�C�����J�n����悤�ɐݒ�
    public void fadeInStart(int red, int green, int blue, int alfa)
    {
        fadeImage.enabled = true;
        SetRGBA(red, green, blue, alfa);
        SetColor();
        isFadeIn = true;
    }

    public void fadeOutStart(int red, int green, int blue, int alfa)
    {
        fadeImage.enabled = true;
        SetRGBA(red, green, blue, alfa);
        SetColor();
        isFadeOut = true;
    }

    void Update()
    {
        if (isFadeIn)
        {
            //�s�����x�����X�ɉ�����
            alfa -= fadeSpeed * Time.deltaTime;
            //�ύX���������x���摜�ɔ��f������֐����Ă�
            SetColor();
            if (alfa <= 0)
            {
                isFadeIn = false;
                fadeImage.enabled = false;
            }
        }
        if (isFadeOut)
        {
            //�s�����x�����X�ɏグ��
            alfa += fadeSpeed * Time.deltaTime;
            //�ύX���������x���摜�ɔ��f������֐����Ă�
            SetColor();
            if (alfa >= 1)
            {
                isFadeOut = false;
            }
        }
    }

    //�摜�ɐF��������֐�
    void SetColor()
    {
        fadeImage.color = new Color(red, green, blue, alfa);
    }

    //�F�̒l��ݒ肷�邽�߂̊֐�
    public void SetRGBA(int r, int g, int b, int a)
    {
        red = r;
        green = g;
        blue = b;
        alfa = a;
    }
}
