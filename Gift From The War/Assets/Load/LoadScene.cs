using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadManager.Instance.LoadNextScene());
    }

    void Update()
    {
        
    }
}
