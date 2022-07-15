using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// BGMとSEの管理をするマネージャ。シングルトン。
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    //ボリューム保存用のkeyとデフォルト値
    private const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
    private const string SE_VOLUME_KEY = "SE_VOLUME_KEY";
    private const float BGM_VOLUME_DEFULT = 1.0f;
    private const float SE_VOLUME_DEFULT = 1.0f;


    //オーディオファイルのパス
    private const string BGM_PATH = "Audio/BGM";
    private const string SE_PATH = "Audio/SE";


    //BGMがフェードするのにかかる時間
    public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
    public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
    private float _bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

    //次流すBGM名、SE名
    private string _nextBGMName;
    private string _nextSEName;

    //SEの設定用変数
    private AudioSource _nowAudio;
    private float _SEVol;
    private float _SEVolSetting = SE_VOLUME_DEFULT;
    private bool _isLoop;

    //BGMをフェードアウト中か
    private bool _isFadeOut = false;
    private float _nextVol;

    //BGM用、SE用に分けてオーディオソースを持つ
    private AudioSource _bgmSource;
    private List<AudioSource> _seSourceList;
    private const int SE_SOURCE_NUM = 100;

    //全AudioClipを保持
    private Dictionary<string, AudioClip> _bgmDic, _seDic;

    //=================================================================================
    //初期化
    //=================================================================================


    protected override void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }


        DontDestroyOnLoad(this.gameObject);

        //オーディオリスナーおよびオーディオソースをSE+1(BGMの分)作成
        for (int i = 0; i < SE_SOURCE_NUM + 1; i++)
        {
            gameObject.AddComponent<AudioSource>();
        }

        //作成したオーディオソースを取得して各変数に設定、ボリュームも設定
        AudioSource[] audioSourceArray = GetComponents<AudioSource>();
        _seSourceList = new List<AudioSource>();

        for (int i = 0; i < audioSourceArray.Length; i++)
        {
            audioSourceArray[i].playOnAwake = false;

            if (i == 0)
            {
                audioSourceArray[i].loop = true;
                _bgmSource = audioSourceArray[i];
                _bgmSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
            }
            else
            {
                _seSourceList.Add(audioSourceArray[i]);
                audioSourceArray[i].volume = PlayerPrefs.GetFloat(SE_VOLUME_KEY, SE_VOLUME_DEFULT);
            }

        }

        //リソースフォルダから全SE&BGMのファイルを読み込みセット
        _bgmDic = new Dictionary<string, AudioClip>();
        _seDic = new Dictionary<string, AudioClip>();

        object[] bgmList = Resources.LoadAll(BGM_PATH);
        object[] seList = Resources.LoadAll(SE_PATH);

        foreach (AudioClip bgm in bgmList)
        {
            _bgmDic[bgm.name] = bgm;
        }
        foreach (AudioClip se in seList)
        {
            _seDic[se.name] = se;
        }
    }

    //=================================================================================
    //SE
    //=================================================================================

    /// <summary>
    /// 指定したファイル名のSEを流す。第二引数のdelayに指定した時間だけ再生までの間隔を空ける
    /// </summary>
    public void PlaySE(string seName, bool isLoop = true, float delay = 0.0f, float vol = 1.0f)
    {
        if (!_seDic.ContainsKey(seName))
        {
            Debug.Log(seName + "という名前のSEがありません");
            return;
        }

        _nextSEName = seName;
        _SEVol = vol;
        _isLoop = isLoop;

        int i = 0;
        foreach (var seDic in _seDic)
        {
            if (seDic.Key == _nextSEName) break;
            i++;
        }
        _nowAudio = _seSourceList[i];

        if (delay != 0)
        {
            Invoke("DelayPlaySE", delay);
        }
        else
        {
            DelayPlaySE();
        }
    }

    public void PlaySE(string seName, GameObject obj, float maxDistance = 100, bool isLiner = true, bool isLoop = true, float delay = 0.0f, float vol = 1.0f)
    {
        if (!_seDic.ContainsKey(seName))
        {
            Debug.Log(seName + "という名前のSEがありません");
            return;
        }

        _nextSEName = seName;
        _SEVol = vol;
        _isLoop = isLoop;

        if (obj.GetComponent<AudioSource>() == null)
        {
            obj.AddComponent<AudioSource>();
        }
        _nowAudio = obj.GetComponent<AudioSource>();
        _nowAudio.spatialBlend = 1;

        _nowAudio.maxDistance = maxDistance;
        _nowAudio.clip = _seDic[seName];

        if (isLiner)
            _nowAudio.rolloffMode = AudioRolloffMode.Linear;


        if (delay != 0)
        {
            Invoke("DelayPlaySE", delay);
        }
        else
        {
            DelayPlaySE();
        }
    }

    private void DelayPlaySE()
    {
        if (!_nowAudio.isPlaying)
        {
            _nowAudio.PlayOneShot(_seDic[_nextSEName] as AudioClip);
            SESetteing(_nowAudio);
            return;
        }
    }

    private void SESetteing(AudioSource audio)
    {
        audio.volume = _SEVol * _SEVolSetting;

        audio.loop = _isLoop;

        audio.minDistance = 1;
    }

    public void StopSE(string seName)
    {
        if (!_seDic.ContainsKey(seName))
        {
            Debug.Log(seName + "という名前のSEがありません");
            return;
        }

        int i = 0;
        foreach (var seDic in _seDic)
        {
            if (seDic.Key == seName) break;
            i++;
        }

        _seSourceList[i].Stop();
    }

    public void StopSE(string seName, GameObject obj)
    {
        if (!_seDic.ContainsKey(seName))
        {
            Debug.Log(seName + "という名前のSEがありません");
            return;
        }

        obj.GetComponent<AudioSource>().Stop();
    }

    //=================================================================================
    //BGM
    //=================================================================================

    /// <summary>
    /// 指定したファイル名のBGMを流す。ただし既に流れている場合は前の曲をフェードアウトさせてから。
    /// 第二引数のfadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
    /// </summary>
    public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH, float vol = 1.0f)
    {
        if (!_bgmDic.ContainsKey(bgmName))
        {
            Debug.Log(bgmName + "という名前のBGMがありません");
            return;
        }

        //現在BGMが流れていない時はそのまま流す
        if (!_bgmSource.isPlaying)
        {
            _nextBGMName = "";
            _bgmSource.clip = _bgmDic[bgmName] as AudioClip;
            _bgmSource.volume = vol;
            _bgmSource.Play();
        }
        //違うBGMが流れている時は、流れているBGMをフェードアウトさせてから次を流す。同じBGMが流れている時はスルー
        else if (_bgmSource.clip.name != bgmName)
        {
            _nextBGMName = bgmName;
            _nextVol = vol;
            FadeOutBGM(fadeSpeedRate);
        }
    }

    /// <summary>
    /// BGMをすぐに止める
    /// </summary>
    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    /// <summary>
    /// 現在流れている曲をフェードアウトさせる
    /// fadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
    /// </summary>
    public void FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW)
    {
        _bgmFadeSpeedRate = fadeSpeedRate;
        _isFadeOut = true;
    }

    private void Update()
    {
        if (!_isFadeOut)
        {
            return;
        }

        //徐々にボリュームを下げていき、ボリュームが0になったらボリュームを戻し次の曲を流す
        _bgmSource.volume -= Time.deltaTime * _bgmFadeSpeedRate;
        if (_bgmSource.volume <= 0)
        {
            _bgmSource.Stop();
            _bgmSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
            _isFadeOut = false;

            if (!string.IsNullOrEmpty(_nextBGMName))
            {
                PlayBGM(_nextBGMName, vol: _nextVol);
            }
        }

    }

    //=================================================================================
    //音量変更
    //=================================================================================

    /// <summary>
    /// BGMとSEのボリュームを別々に変更&保存
    /// </summary>
    public void ChangeBGMVolume(float BGMVolume)
    {
        _bgmSource.volume = BGMVolume;

        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVolume);
    }

    public void ChangeSEVolume(float SEVolume)
    {
        _SEVolSetting = SEVolume;

        PlayerPrefs.SetFloat(SE_VOLUME_KEY, SEVolume);
    }

    public float GetBGMVolume()
    {
        return _bgmSource.volume;
    }

    public float GetSEVolume()
    {
        return _SEVolSetting;
    }

}
