using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject image;
    [SerializeField] List<ButtonReactionAnime> list;


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
            if (_pauseFlg != pauseFlg)
            {
                Pause();
            }
        }
        else
        {
            if (_pauseFlg != pauseFlg)
            {
                Resume();
            }
        }
    }

    private void Pause()
    {
        SystemSetting.Instance.Pause(); // ���Ԓ�~
        image.SetActive(true);
        CursorManager.Instance.cursorLock = false;
    }

    private void Resume()
    {
        SystemSetting.Instance.Resume();  // �ĊJ
        image.SetActive(false);
        CursorManager.Instance.cursorLock = true;

        foreach (var lists in list)
        {
            lists.AnimReset();
        }
    }

    public void ChangeTitle()
    {
        Resume();
        StartCoroutine(LoadManager.Instance.LoadScene("Scenes/TitleScene"));
        pauseFlg = false;
        image.SetActive(false);
        CursorManager.Instance.cursorLock = false;

        if (TimeAttackManager.Instance.timerStartFlg) TimeAttackManager.Instance.TimerFinish();
    }

    public void BackGame()
    {
        Resume();
        image.SetActive(false);
        pauseFlg = false;
        CursorManager.Instance.cursorLock = true;
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
