using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ScenarioData : SingletonMonoBehaviour<ScenarioData>
{
    public struct SaveData
    {
        public int talkCount;
        public int neziKunCount;
        public int maxCount;
    }

   [System.NonSerialized] public bool enable=true;

    public SaveData saveData;

    private string filePath;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

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
        
        //デバッグ用
        if(Input.GetKeyDown(KeyCode.K))
        {
            saveData.talkCount = 0;
            saveData.neziKunCount = 0;
           // saveData.maxCount = 0;

            WriteFile();
        }
    }


    //ファイル読み込み
    public void ReadFile()
    {
        if (!File.Exists(filePath)) return;

        using(var reader =new BinaryReader(new FileStream(filePath,FileMode.Open)))
        {
            saveData.talkCount = reader.ReadInt32();
            saveData.neziKunCount = reader.ReadInt32();
            saveData.maxCount = reader.ReadInt32();

        }

        Debug.Log("max=" + saveData.maxCount);
        Debug.Log("nezi=" + saveData.neziKunCount);
      //  Debug.Log("max=" + saveData.maxCount);

    }

    //ファイル書き込み 
    public void WriteFile()
    {
        if (!File.Exists(filePath)) return;

        if(saveData.talkCount==0)
        {
            saveData.talkCount = 0;
        }

        using (var writer =new BinaryWriter(new FileStream(filePath,FileMode.Create)))
        {
            if(saveData.neziKunCount>0)
            {
                writer.Write(saveData.talkCount-1);
                writer.Write(saveData.neziKunCount-1);
                writer.Write(saveData.maxCount);
            }

            if (saveData.neziKunCount==0)
            {
                saveData.neziKunCount = 0;

                writer.Write(saveData.talkCount);
                writer.Write(saveData.neziKunCount);
                writer.Write(saveData.maxCount);
            }

            
        }
    }
}
