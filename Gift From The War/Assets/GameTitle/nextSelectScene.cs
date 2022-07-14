using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextSelectScene : MonoBehaviour
{

    bool isCalledOnce = false;

    private void Update()
    {
        //マウス
        if (Input.anyKey && !isCalledOnce)
        {
            isCalledOnce = true;
            AudioManager.Instance.PlaySE("決定ボタンを押す10", isLoop: false);
            StartCoroutine(nextScene());
        }
    }

    private IEnumerator nextScene()
    {

        FadeManager.Instance.fadeOutStart(0, 0, 0, 0);

        var asyncOperation = SceneManager.LoadSceneAsync("Scenes/TitleScene");

        asyncOperation.allowSceneActivation = false;

        while (FadeManager.Instance.isFadeOut)
        {
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;

        FadeManager.Instance.fadeInStart(0, 0, 0, 1);
    }
}
