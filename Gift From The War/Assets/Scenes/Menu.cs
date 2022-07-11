using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject image;

    private bool pauseFlg;
    void Start()
    {
        image.SetActive(false);
        pauseFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool _pauseFlg = pauseFlg;


        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            if (SceneManager.GetActiveScene().name == "FirstScene" || SceneManager.GetActiveScene().name == "SecondScene" || SceneManager.GetActiveScene().name == "FinalStage")
            {
                pauseFlg = !pauseFlg;
            }
        }

        if (pauseFlg == true)
        {
            Pause();
        }
        else
        {
            if(_pauseFlg != pauseFlg) Resume();
        }

    }

    private void Pause()
    {
        Time.timeScale = 0;  // ���Ԓ�~
        image.SetActive(true);
    }

    private void Resume()
    {
        Time.timeScale = 1;  // �ĊJ
        image.SetActive(false);
    }

    public void ChangeTitle()
    {
        StartCoroutine(LoadManager.Instance.LoadScene("Scenes/TitleScene"));
        pauseFlg = false;
        image.SetActive(false);
        Resume();
    }

    public void BackGame()
    {
        Resume();
        image.SetActive(false);
        pauseFlg = false;
    }

    //�Q�[���I��
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
        Application.Quit();//�Q�[���v���C�I��
#endif
    }
}
