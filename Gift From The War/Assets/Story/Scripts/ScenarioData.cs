using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ScenarioData : SingletonMonoBehaviour<ScenarioData>
{
    private string filePath;

    //



    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.dataPath + @"\Story\Binary\ScenarioData";

        if(!File.Exists(filePath))
        {
            using(File.Create(filePath))
            {

            }
        }

        ReadFile();
    }

    // Update is called once per frame
    void Update()
    {
        
        //�f�o�b�O�p
        if(Input.GetKeyDown(KeyCode.K))
        {
            ScenarioManager.Instance.talkCount = 0;
            WriteFile();
        }
    }


    //�t�@�C���ǂݍ���
    public void ReadFile()
    {
        if (!File.Exists(filePath)) return;
        using(var reader =new BinaryReader(new FileStream(filePath,FileMode.Open)))
        {
            ScenarioManager.Instance.talkCount = reader.ReadInt32();
        }
    }

    //�t�@�C���������� 
    public void WriteFile()
    {
        if (!File.Exists(filePath)) return;
        using(var writer =new BinaryWriter(new FileStream(filePath,FileMode.Create)))
        {
            writer.Write(ScenarioManager.Instance.talkCount);

        }
    }
}
