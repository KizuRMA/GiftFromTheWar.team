using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : SingletonMonoBehaviour<LoadManager>
{
    [SerializeField] private float startLoadTime;
    [SerializeField] private float finishLoadTime;
    private string nextScenePath;
    private bool startLoadFlg = false;
    private bool finishLoadFlg = false;
    public bool loadFlg { get; set; }

    void Start()
    {

    }

    public void Load()
    {
        StartCoroutine("StartLoad");
    }

    public void Load(string _nextScenePath)
    {
        nextScenePath = _nextScenePath;

        StartCoroutine("FinishLoad");
    }

    private IEnumerator StartLoad()
    {
        startLoadFlg = true;

        yield return new WaitForSeconds(startLoadTime);

        startLoadFlg = false;
    }

    private IEnumerator FinishLoad()
    {
        finishLoadFlg = true;

        yield return new WaitForSeconds(finishLoadTime);

        finishLoadFlg = false;
        SceneManager.LoadScene(nextScenePath);
    }

    void Update()
    {
        loadFlg = startLoadFlg || finishLoadFlg;
    }
}