using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    //SE�̉���
    private float _SEVol;

    //BGM���t�F�[�h�A�E�g����
    private bool _isFadeOut = false;

    //BGM�p�ASE�p�ɕ����ăI�[�f�B�I�\�[�X������
    private AudioSource _bgmSource;
    private List<AudioSource> _seSourceList;
    private const int SE_SOURCE_NUM = 10;

    //�SAudioClip��ێ�
    private Dictionary<string, AudioClip> _bgmDic, _seDic;

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

        DontDestroyOnLoad(this.gameObject);

        //�I�[�f�B�I���X�i�[����уI�[�f�B�I�\�[�X��SE+1(BGM�̕�)�쐬
        gameObject.AddComponent<AudioListener>();
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
    public void PlaySE(string seName, float delay = 0.0f, float vol = 1.0f)
    {
        if (!_seDic.ContainsKey(seName))
        {
            Debug.Log(seName + "�Ƃ������O��SE������܂���");
            return;
        }

        _nextSEName = seName;
        _SEVol = vol;
        Invoke("DelayPlaySE", delay);
    }

    private void DelayPlaySE()
    {
        int i = 0;
        foreach (var seDic in _seDic)
        {
            if (seDic.Key == _nextSEName) break;
            i++;
        }

        if (!_seSourceList[i].isPlaying)
        {
            _seSourceList[i].PlayOneShot(_seDic[_nextSEName] as AudioClip);
            _seSourceList[i].volume = _SEVol;
            return;
        }
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
            _bgmSource.volume = vol;
            _bgmSource.Play();
        }
        //�ႤBGM������Ă��鎞�́A����Ă���BGM���t�F�[�h�A�E�g�����Ă��玟�𗬂��B����BGM������Ă��鎞�̓X���[
        else if (_bgmSource.clip.name != bgmName)
        {
            _nextBGMName = bgmName;
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
                PlayBGM(_nextBGMName);
            }
        }

    }

    //=================================================================================
    //���ʕύX
    //=================================================================================

    /// <summary>
    /// BGM��SE�̃{�����[����ʁX�ɕύX&�ۑ�
    /// </summary>
    public void ChangeVolume(float BGMVolume, float SEVolume)
    {
        _bgmSource.volume = BGMVolume;
        foreach (AudioSource seSource in _seSourceList)
        {
            seSource.volume = SEVolume;
        }

        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVolume);
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, SEVolume);
    }

}
