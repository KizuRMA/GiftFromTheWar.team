using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextSelectScene : MonoBehaviour
{

    bool isCalledOnce = false;

    private void Update()
    {
        //É}ÉEÉX
        if (Input.anyKey && !isCalledOnce)
        {
            isCalledOnce = true;
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
