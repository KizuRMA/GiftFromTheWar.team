using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AchievementManager : SingletonMonoBehaviour<AchievementManager>
{
    public enum AchievementType
    {
        BAD_CLEAR,
        DOG_CLEAR,
        BOSS_CLEAR,
        NO_GUN,
        NO_BULLET_WIND,
        NO_BULLET_MAGNET,
        NO_BULLET_FIRE,
        TIMEATTACK_ARANK
    }

    public struct AchievementData
    {
        public bool badData;
        public bool dogData;
        public bool bossData;
        public bool noGunFlg;
        public bool noWindFlg;
        public bool noMagnetFlg;
        public bool noFireFlg;
        public bool timeAttackAFlg;
    }

    public AchievementData nowAchievementData;

    private string directoryName;
    private string fileSaveSpotName;
    private string path;

    public bool achievementJudgeStartFlg;

    void Start()
    {
        directoryName = "binaryFolder";
        fileSaveSpotName = "binaryAchiveData.GftW";
        path = directoryName + "/" + fileSaveSpotName;

        if (!System.IO.Directory.Exists(directoryName))
        {
            System.IO.Directory.CreateDirectory(directoryName);
        }

        if (!System.IO.File.Exists(path))
        {
            System.IO.File.Create(path);
        }
        else
        {
            ReadFile();
        }

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.H))
        {
            Debug.Log(nowAchievementData.badData);
        }

        if(achievementJudgeStartFlg)
        {
            //アチーブメントを判定するための変数をリセット
            ReadFile();
        }

        if (!achievementJudgeStartFlg) return;
    }

    public void ReadFile()
    {
        if ((!System.IO.Directory.Exists(directoryName)) || (!System.IO.File.Exists(path))) return;

        using (var reader = new BinaryReader(new FileStream(path, FileMode.Open)))
        {
            nowAchievementData.badData = reader.ReadBoolean();
            nowAchievementData.dogData = reader.ReadBoolean();
            nowAchievementData.bossData = reader.ReadBoolean();
            nowAchievementData.noGunFlg = reader.ReadBoolean();
            nowAchievementData.noWindFlg = reader.ReadBoolean();
            nowAchievementData.noMagnetFlg = reader.ReadBoolean();
            nowAchievementData.noFireFlg = reader.ReadBoolean();
            nowAchievementData.timeAttackAFlg = reader.ReadBoolean();
        }
    }

    public void WriteFile()
    {
        if ((!System.IO.Directory.Exists(directoryName)) || (!System.IO.File.Exists(path))) return;

        using (var writer = new BinaryWriter(new FileStream(path, FileMode.Create)))
        {
            //書き込む処理
            writer.Write(nowAchievementData.badData);
            writer.Write(nowAchievementData.dogData);
            writer.Write(nowAchievementData.bossData);
            writer.Write(nowAchievementData.noGunFlg);
            writer.Write(nowAchievementData.noWindFlg);
            writer.Write(nowAchievementData.noMagnetFlg);
            writer.Write(nowAchievementData.noFireFlg);
            writer.Write(nowAchievementData.timeAttackAFlg);
        }
    }
}
