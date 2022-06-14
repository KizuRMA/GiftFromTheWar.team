using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveManager : MonoBehaviour
{
    private string directoryName;
    private string fileSaveSpotName;
    private string path;

    void Start()
    {
        directoryName = "binaryFolder";
        fileSaveSpotName = "binaryData.GftW";
        path = directoryName + "/" + fileSaveSpotName;

        if (!System.IO.Directory.Exists(directoryName))
        {
            System.IO.Directory.CreateDirectory(directoryName);
        }

        if (!System.IO.File.Exists(path))
        {
            System.IO.File.Create(path);
            //new FileStream(path, FileMode.CreateNew);
        }
    }

    void Update()
    {
        WriteFile();

        ReadFile();
    }

    public void ReadFile()
    {
        if ((!System.IO.Directory.Exists(directoryName)) || (!System.IO.File.Exists(path))) return;

        using (var reader = new BinaryReader(new FileStream(path, FileMode.Open)))
        {
            var data1 = reader.ReadBoolean();
        }
    }

    public void WriteFile()
    {
        if ((!System.IO.Directory.Exists(directoryName)) || (!System.IO.File.Exists(path))) return;

        using (var writer = new BinaryWriter(new FileStream(path, FileMode.Create)))
        {
            //èëÇ´çûÇﬁèàóù
            writer.Write((bool)false);
        }
    }
}
