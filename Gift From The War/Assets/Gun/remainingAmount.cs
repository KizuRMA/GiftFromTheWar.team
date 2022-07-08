using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remainingAmount : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    [SerializeField] private GameObject amount;
    [SerializeField] private GameObject amount2;
    [SerializeField] private Transform trans;
    [SerializeField] private Material matWind;
    [SerializeField] private Material matWindMax;
    [SerializeField] private Material matMagnet;
    [SerializeField] private Material matMagnetMax;
    [SerializeField] private Material matFire;
    [SerializeField] private Material matFireMax;
    [SerializeField] private bulletChange bulletChange;

    //�G�l���M�[�c�ʂ̕\��
    private float nowRemainingEnergy;           //���̎c��
    private float useEnergy;                    //�����
    private float allRemainingEnergy;           //�v�Z�ɕK�v�Ȓ萔
    [SerializeField] private float upSpeed;     //�񕜃X�s�[�h
    [SerializeField] private float energyMax;   //�ő��
    [SerializeField] private float energyMin;   //�ŏ���
    public bool useDeltaTime { get; set; }     //�f���^�^�C�����g����

    //�G�l���M�[�񕜂̃N�[���^�C��
    [SerializeField] private int coolTime;      //�G�l���M�[�񕜂̃N�[���^�C��
    private bool recoveryFlg = false;           //�񕜂���t���O
    private IEnumerator cor;                    //�����Ă���R���[�`��

    public bool energyMaxFlg { get; set; } //�G�l���M�[���ő傪�ǂ���

    void Start()
    {
        allRemainingEnergy = energyMax - energyMin;
        energyMaxFlg = false;
    }

    void Update()
    {
        if (useEnergy == 0) //�G�l���M�[����Ȃ�������
        {
            NoUseEnergy();
        }
        else
        {
            UseEnergy();
        }

        EnergyMax();

        ColorChange();

        nowRemainingEnergy = 1.0f - (energyMax - trans.localPosition.z) / allRemainingEnergy;
    }

    private IEnumerator RecoveryCoolTime()  //�񕜂܂ł̃N�[���^�C��
    {
        yield return new WaitForSeconds(coolTime);  //�N�[���^�C�����҂�

        recoveryFlg = true;
    }

    private void NoUseEnergy()  //�G�l���M�[����Ȃ�������
    {
        //�G�l���M�[�̉�
        if (recoveryFlg)
        {
            trans.localPosition += new Vector3(0, 0, -(upSpeed * allRemainingEnergy - allRemainingEnergy) * Time.deltaTime);
        }
    }

    private void UseEnergy()    //�G�l���M�[�����������
    {
        recoveryFlg = false;
        energyMaxFlg = false;

        //��̃R���[�`�����g���܂킷
        if (cor != null) StopCoroutine(cor);
        cor = null;
        cor = RecoveryCoolTime();
        StartCoroutine(cor);

        //�G�l���M�[�̏����
        if (useDeltaTime)
        {
            trans.localPosition += new Vector3(0, 0, (useEnergy * allRemainingEnergy - allRemainingEnergy) * Time.deltaTime);
        }
        else
        {
            trans.localPosition += new Vector3(0, 0, (useEnergy * allRemainingEnergy - allRemainingEnergy));
        }
    }

    private void EnergyMax()    //�G�l���M�[���ő�܂ł��܂��Ă�����
    {
        if (trans.localPosition.z > energyMax)
        {
            //�G�l���M�[�̍ő�l
            trans.localPosition = new Vector3(0, 0, energyMax);

            energyMaxFlg = true;
        }
        else if (trans.localPosition.z < energyMin)
        {
            //�G�l���M�[�̍ŏ��l
            trans.localPosition = new Vector3(0, 0, energyMin);
        }
    }

    private void ColorChange()  //�G�l���M�[�̐F�ύX
    {
        if (bulletChange.cylinder.isChanging == true) return;

        Material[] tmp = gameObject.GetComponent<Renderer>().materials;

        if (trans.localPosition.z < energyMax)
        {
            //�e�̎�ނɂ��A�F�̕ω�
            switch (bulletChange.nowBulletType)
            {
                case bulletChange.bulletType.e_wind:
                    tmp[0] = matWind;
                    break;

                case bulletChange.bulletType.e_magnet:
                    tmp[0] = matMagnet;
                    break;

                case bulletChange.bulletType.e_fire:
                    tmp[0] = matFire;
                    break;
            }
        }
        else
        {
            //�e�̎�ނɂ��A�F�̕ω�
            switch (bulletChange.nowBulletType)
            {
                case bulletChange.bulletType.e_wind:
                    tmp[0] = matWindMax;
                    break;

                case bulletChange.bulletType.e_magnet:
                    tmp[0] = matMagnetMax;
                    break;

                case bulletChange.bulletType.e_fire:
                    tmp[0] = matFireMax;
                    break;
            }
        }

        amount2.GetComponent<Renderer>().materials = tmp;
    }

    public float GetSetNowAmount
    {
        get { return nowRemainingEnergy; }
        set { useEnergy = value; }
    }
}
