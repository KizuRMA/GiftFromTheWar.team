using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// BGM��SE�̊Ǘ�������}�l�[�W���B�V���O���g���B
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    //�{�����[���ۑ��p��key�ƃf�t�H���g�l
    private const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
    private const string SE_VOLUME_KEY = "SE_VOLUME_KEY";
    private const float BGM_VOLUME_DEFULT = 1.0f;
    private const float SE_VOLUME_DEFULT = 1.0f;


    //�I�[�f�B�I�t�@�C���̃p�X
    private const string BGM_PATH = "Audio/BGM";
    private const string SE_PATH = "Audio/SE";


    //BGM���t�F�[�h����̂ɂ����鎞��
    public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
    public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
    private float _bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

    //������BGM���ASE��
    private string _nextBGMName;
    private string _nextSEName;

    //SE�̐ݒ�p�ϐ�
    private AudioSource _nowAudio;
    private float _SEVol;
    private float _SEVolSetting = SE_VOLUME_DEFULT;
    private bool _isLoop;

    //BGM���t�F�[�h�A�E�g����
    private bool _isFadeOut = false;
    private float _BGMVolSetting = BGM_VOLUME_DEFULT;
    private float _nextVol;

    //BGM�p�ASE�p�ɕ����ăI�[�f�B�I�\�[�X������
    private AudioSource _bgmSource;
    private List<AudioSource> _seSourceList;
    private const int SE_SOURCE_NUM = 100;

    //�SAudioClip��ێ�
    private Dictionary<string, AudioClip> _bgmDic, _seDic;

    private string directoryName = "binaryFolder";
    private string fileSaveSpotName = "Setting.GftW";
    private string path;

    //=================================================================================
    //������
    //=================================================================================


    protected override void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        //�Z�[�u�f�[�^�֌W
        path = directoryName + "/" + fileSaveSpotName;

        if (!System.IO.Directory.Exists(directoryName))
        {
            System.IO.Directory.CreateDirectory(directoryName);
        }

        if (!System.IO.File.Exists(path))
        {
            System.IO.File.Create(path);

            _BGMVolSetting = BGM_VOLUME_DEFULT;
            _SEVolSetting = SE_VOLUME_DEFULT;

            StartCoroutine(FirstWrite());
        }
        else
        {
            ReadFile();
        }

        DontDestroyOnLoad(this.gameObject);

        //�I�[�f�B�I���X�i�[����уI�[�f�B�I�\�[�X��SE+1(BGM�̕�)�쐬
        for (int i = 0; i < SE_SOURCE_NUM + 1; i++)
        {
            gameObject.AddComponent<AudioSource>();
        }

        //�쐬�����I�[�f�B�I�\�[�X���擾���Ċe�ϐ��ɐݒ�A�{�����[�����ݒ�
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

        //���\�[�X�t�H���_����SSE&BGM�̃t�@�C����ǂݍ��݃Z�b�g
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
    /// �w�肵���t�@�C������SE�𗬂��B��������delay�Ɏw�肵�����Ԃ����Đ��܂ł̊Ԋu���󂯂�
    /// </summary>
    public void PlaySE(string seName, bool isLoop = true, float delay = 0.0f, float vol = 1.0f)
    {
        if (!_seDic.ContainsKey(seName))
        {
            Debug.Log(seName + "�Ƃ������O��SE������܂���");
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
            Debug.Log(seName + "�Ƃ������O��SE������܂���");
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
            Debug.Log(seName + "�Ƃ������O��SE������܂���");
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
            Debug.Log(seName + "�Ƃ������O��SE������܂���");
            return;
        }

        obj.GetComponent<AudioSource>().Stop();
    }

    //=================================================================================
    //BGM
    //=================================================================================

    /// <summary>
    /// �w�肵���t�@�C������BGM�𗬂��B���������ɗ���Ă���ꍇ�͑O�̋Ȃ��t�F�[�h�A�E�g�����Ă���B
    /// ��������fadeSpeedRate�Ɏw�肵�������Ńt�F�[�h�A�E�g����X�s�[�h���ς��
    /// </summary>
    public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH, float vol = 1.0f)
    {
        if (!_bgmDic.ContainsKey(bgmName))
        {
            Debug.Log(bgmName + "�Ƃ������O��BGM������܂���");
            return;
        }

        //����BGM������Ă��Ȃ����͂��̂܂ܗ���
        if (!_bgmSource.isPlaying)
        {
            _nextBGMName = "";
            _bgmSource.clip = _bgmDic[bgmName] as AudioClip;
            _nextVol = vol;
            _bgmSource.volume = vol * _BGMVolSetting;
            _bgmSource.Play();
        }
        //�ႤBGM������Ă��鎞�́A����Ă���BGM���t�F�[�h�A�E�g�����Ă��玟�𗬂��B����BGM������Ă��鎞�̓X���[
        else if (_bgmSource.clip.name != bgmName)
        {
            _nextBGMName = bgmName;
            _nextVol = vol;
            FadeOutBGM(fadeSpeedRate);
        }
    }

    /// <summary>
    /// BGM�������Ɏ~�߂�
    /// </summary>
    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    /// <summary>
    /// ���ݗ���Ă���Ȃ��t�F�[�h�A�E�g������
    /// fadeSpeedRate�Ɏw�肵�������Ńt�F�[�h�A�E�g����X�s�[�h���ς��
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

        //���X�Ƀ{�����[���������Ă����A�{�����[����0�ɂȂ�����{�����[����߂����̋Ȃ𗬂�
        _bgmSource.volume -= Time.deltaTime * _bgmFadeSpeedRate;
        if (_bgmSource.volume <= 0)
        {
            _bgmSource.Stop();
            _bgmSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
            _isFadeOut = false;

            if (!string.IsNullOrEmpty(_nextBGMName))
            {
                PlayBGM(_nextBGMName, vol: _nextVol * _BGMVolSetting);
            }
        }

    }

    //=================================================================================
    //���ʕύX
    //=================================================================================

    /// <summary>
    /// BGM��SE�̃{�����[����ʁX�ɕύX&�ۑ�
    /// </summary>
    public void ChangeBGMVolume(float BGMVolume)
    {
        _BGMVolSetting = BGMVolume;

        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVolume);

        if (_isFadeOut) return;

        _bgmSource.volume = _nextVol * _BGMVolSetting;
    }

    public void ChangeSEVolume(float SEVolume)
    {
        _SEVolSetting = SEVolume;

        PlayerPrefs.SetFloat(SE_VOLUME_KEY, SEVolume);
    }

    public float GetBGMVolume()
    {
        return _BGMVolSetting;
    }

    public float GetSEVolume()
    {
        return _SEVolSetting;
    }

    //�Z�[�u�f�[�^�֌W
    public void ReadFile()
    {
        if ((!System.IO.Directory.Exists(directoryName)) || (!System.IO.File.Exists(path))) return;

        using (var reader = new BinaryReader(new FileStream(path, FileMode.Open)))
        {
            _BGMVolSetting = (float)reader.ReadDouble();
            _SEVolSetting = (float)reader.ReadDouble();
        }
    }

    public void WriteFile()
    {
        if ((!System.IO.Directory.Exists(directoryName)) || (!System.IO.File.Exists(path))) return;

        using (var writer = new BinaryWriter(new FileStream(path, FileMode.Create)))
        {
            //�������ޏ���
            writer.Write((double)_BGMVolSetting);
            writer.Write((double)_SEVolSetting);
        }
    }

    private IEnumerator FirstWrite()
    {
        yield return new WaitForSeconds(1);

        WriteFile();
    }

}
