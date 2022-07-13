using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    [SerializeField] private CanvasGroup CanvasGroup;

    //�A���N���b�N�h�~
    bool isCalledOnce = false;

    private void Start()
    {
        Initialize();
        TextApp();
    }

    private void Update()
    {
        //��������{�^������������Z���N�g��ʂɈړ�
        if (Input.anyKey && !isCalledOnce)
        {
            isCalledOnce = true;
            //StartCoroutine(nextScene());
        }
    }

    //�e�L�X�g�����t�F�[�h�C������A�j���[�V����
    public void TextApp()
    {
        CanvasGroup.DOFade(endValue: 1f, duration: 0.2f);

    }

    //��͔C�����B

    //private IEnumerator nextScene()
    //{
    //    FadeManager.Instance.fadeOutStart(0, 0, 0, 0);

    //    var asyncOperation = SceneManager.LoadSceneAsync("Scenes/TitleScene");

    //    asyncOperation.allowSceneActivation = false;

    //    while (FadeManager.Instance.isFadeOut)
    //    {
    //        yield return null;
    //    }

    //    asyncOperation.allowSceneActivation = true;

    //    FadeManager.Instance.fadeInStart(0, 0, 0, 1);
    //}

    private void Initialize()
    {
        CanvasGroup.alpha = 0.0f;
    }

}
