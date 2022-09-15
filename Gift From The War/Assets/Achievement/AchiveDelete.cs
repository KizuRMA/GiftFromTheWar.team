using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AchiveDelete : MonoBehaviour
{
    private string directoryName = "binaryFolder";
    private string fileSaveSpotName = "binaryAchiveData.GftW";
    private string path;

    // Start is called before the first frame update
    void Start()
    {
        path = directoryName + "/" + fileSaveSpotName;
    }

    // Update is called once per frame
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
