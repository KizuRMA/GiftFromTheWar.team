using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FedeCanvas : MonoBehaviour
{
    [SerializeField]GameObject image;
    private void Start()
    {

    }

    void Update()
    {
        bool _active = !((SceneManager.GetActiveScene().name == "LoadScene"));
        image.SetActive(_active);
    }


}
