using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearScript : MonoBehaviour
{
    private float time;
    [SerializeField]private float showTime;

    bool isCalledOnce = false;
    void Start()
    {
        time = 0;
        CursorManager.Instance.SetCursorLock(false);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time <= showTime) return;

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
