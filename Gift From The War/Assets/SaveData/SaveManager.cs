using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveManager : SingletonMonoBehaviour<SaveManager>
{
    public enum SaveSpotNum
    {
        none,
        s1p1,
        s1p2,
        s1p3,
        s1p4,
        s1p5,
        s2p1,
        s2p2,
        s2p3,
        s2p4,
        s2p5,
        s3p1,
        s3p2,
        s3p3,
        s3p4
    }

    public struct SaveData
    {
        public SaveSpotNum saveSpotNum;
        public Vector3 dataSpotPos;
        public Vector3 goalPos;
        public bool getGunFlg;
        public bool getRantanFlg;
        public bool getWindFlg;
        public bool getMagnetFlg;
        public bool getFireFlg;
    }

    public SaveData nowSaveData;

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
        }

        DontDestroyOnLoad(this);
    }

    void Update()
    {
    }

    public void ReadFile()
    {
        if ((!System.IO.Directory.Exists(directoryName)) || (!System.IO.File.Exists(path))) return;

        using (var reader = new BinaryReader(new FileStream(path, FileMode.Open)))
        {
            nowSaveData.saveSpotNum = (SaveSpotNum)reader.ReadUInt32();
            nowSaveData.dataSpotPos = new Vector3((float)reader.ReadDouble(), (float)reader.ReadDouble(), (float)reader.ReadDouble());
            nowSaveData.goalPos = new Vector3((float)reader.ReadDouble(), (float)reader.ReadDouble(), (float)reader.ReadDouble());
            nowSaveData.getGunFlg = reader.ReadBoolean();
            nowSaveData.getRantanFlg = reader.ReadBoolean();
            nowSaveData.getWindFlg = reader.ReadBoolean();
            nowSaveData.getMagnetFlg = reader.ReadBoolean();
            nowSaveData.getFireFlg = reader.ReadBoolean();
        }
    }

    public void WriteFile()
    {
        if ((!System.IO.Directory.Exists(directoryName)) || (!System.IO.File.Exists(path))) return;

        using (var writer = new BinaryWriter(new FileStream(path, FileMode.Create)))
        {
            //èëÇ´çûÇﬁèàóù
            writer.Write((uint)nowSaveData.saveSpotNum);
            writer.Write((double)nowSaveData.dataSpotPos.x);
            writer.Write((double)nowSaveData.dataSpotPos.y);
            writer.Write((double)nowSaveData.dataSpotPos.z);
            writer.Write((double)nowSaveData.goalPos.x);
            writer.Write((double)nowSaveData.goalPos.y);
            writer.Write((double)nowSaveData.goalPos.z);
            writer.Write(nowSaveData.getGunFlg);
            writer.Write(nowSaveData.getRantanFlg);
            writer.Write(nowSaveData.getWindFlg);
            writer.Write(nowSaveData.getMagnetFlg);
            writer.Write(nowSaveData.getFireFlg);
        }
    }

    public void Restart()
    {
        nowSaveData.saveSpotNum = SaveSpotNum.none;
        nowSaveData.dataSpotPos = Vector3.zero;
        nowSaveData.goalPos = Vector3.zero;
        nowSaveData.getGunFlg = false;
        nowSaveData.getRantanFlg = false;
        nowSaveData.getWindFlg = false;
        nowSaveData.getMagnetFlg = false;
        nowSaveData.getFireFlg = false;
    }
}
