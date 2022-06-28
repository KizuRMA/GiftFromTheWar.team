using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : SingletonMonoBehaviour<LoadManager>
{
    [SerializeField] private float loadRaitomax;
    [SerializeField] private float loadTime;
    private string nextSceneName;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadScene(string nextScene)
    {
        SceneManager.LoadScene("Scenes/LoadScene");
        nextSceneName = nextScene;
    }

    public IEnumerator LoadNextScene()
    {
        var asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);

        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < loadRaitomax)
        {
            yield return null;
        }

        yield return new WaitForSeconds(loadTime);

        asyncOperation.allowSceneActivation = true;
    }
}