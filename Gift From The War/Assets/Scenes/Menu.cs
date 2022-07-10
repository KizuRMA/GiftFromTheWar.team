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
            pauseFlg = !pauseFlg;
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
        Time.timeScale = 0;  // éûä‘í‚é~
        image.SetActive(true);
    }

    private void Resume()
    {
        Time.timeScale = 1;  // çƒäJ
        image.SetActive(false);
    }

    public void ChangeTitle()
    {
        SceneManager.LoadScene("Scenes/TitleScene");
    }
}
