using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeAttackManager : SingletonMonoBehaviour<TimeAttackManager>
{
    public bool timeAttackFlg { get; set; } //�^�C���A�^�b�N���s�����ǂ���
    public bool timerStartFlg { get; set; } //�^�C�}�[�����X�^�[�g���邩
    public bool timerStopFlg { get; set; } //�^�C�}�[�����X�^�[�g���邩
    public bool playerDiedFlg { get; set; } //�v���C���[�����񂾂��ǂ���
    [SerializeField] private TextMeshProUGUI timer;
    private float countTime = 0;
    private float countSecond = 0;
    private int countMinute = 0;

    void Start()
    {
        DontDestroyOnLoad(this);
        timeAttackFlg = false;
        timerStartFlg = false;
        timerStopFlg = false;
        playerDiedFlg = false;
        timer.enabled = false;
    }

    void Update()
    {
        if (!timeAttackFlg) return;

        if (!timerStartFlg) return;

        if (!timerStopFlg)
        {
            Timer();
        }

        TimeDisplay();
    }

    private void Timer()
    {
        if (countMinute >= 99) return;

        // countTime�ɁA�Q�[�����J�n���Ă���̕b�����i�[
        countTime += Time.deltaTime;

        //countTime����o�ߎ��Ԃ��Z�o
        float time = countTime;
        countMinute = (int)time / 60;
        time = time % 60;
        countSecond = time;
    }

    private void TimeDisplay()
    {
        timer.enabled = true;

        // ����2���ɂ��ĕ\��
        timer.SetText(countMinute.ToString("00") + ":" + countSecond.ToString("00.00"));
    }

    public void TimerFinish()
    {
        TimerReset();
        timer.enabled = false;
        timeAttackFlg = false;
        timerStartFlg = false;
        timerStopFlg = false;
        playerDiedFlg = false;
    }

    public void TimerReset()
    {
        countTime = 0;
    }

    public void TimerHide()
    {
        timer.enabled = false;
    }

    public float Result()
    {
        return countTime;
    }
}
