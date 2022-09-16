using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AchiveDelete : MonoBehaviour
{
    private string directoryName = "binaryFolder";
    private string fileSaveSpotName = "binaryAchiveData.GftW";
    private string path;

    void Start()
    {
        path = directoryName + "/" + fileSaveSpotName;
    }

    void Update()
    {
        
    }

    public void OnClickButton()
    {
        if (System.IO.File.Exists(path))
        {
            File.Delete(path);
        }

        AchievementManager.Instance.NewFile();
    }
}
