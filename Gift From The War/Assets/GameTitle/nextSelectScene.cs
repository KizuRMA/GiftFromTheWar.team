using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextSelectScene : MonoBehaviour
{
    private void Update()
    {
        //É}ÉEÉX
        if (Input.anyKey)
        {
            SceneManager.LoadScene("Scenes/TitleScene");
        }
    }
}
