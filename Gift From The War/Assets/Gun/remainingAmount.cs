using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remainingAmount : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    [SerializeField] private GameObject amount;
    [SerializeField] private GameObject amount2;
    [SerializeField] private Transform trans;
    [SerializeField] private MoveWindGun windGun;
    [SerializeField] private Material mat1;
    [SerializeField] private Material mat2;

    //�G�l���M�[�c�ʂ̕\��
    private Vector3 firstPos;                   //��̈ʒu   
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
        firstPos = transform.position;
        allRemainingEnergy = energyMax - energyMin;
        energyMaxFlg = false;
    }

    void Update()
    {
        if (useEnergy == 0) //�G�l���M�[����Ȃ�������
        {
            //�G�l���M�[�̉�
            if (recoveryFlg)
            {
                trans.localPosition += new Vector3(0, 0, -(upSpeed * allRemainingEnergy - allRemainingEnergy) * Time.deltaTime);
            }
        }
        else
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

            //�G�l���M�[�̐F�ύX
            Material[] tmp = gameObject.GetComponent<Renderer>().materials;
            tmp[0] = mat1;
            amount2.GetComponent<Renderer>().materials = tmp;
        }

        if (trans.localPosition.z > energyMax)
        {
            //�G�l���M�[�̍ő�l
            trans.localPosition = new Vector3(0, 0, energyMax);

            energyMaxFlg = true;

            //�G�l���M�[�̐F�ύX
            Material[] tmp = gameObject.GetComponent<Renderer>().materials;
            tmp[0] = mat2;
            amount2.GetComponent<Renderer>().materials = tmp;
        }
        else if (trans.localPosition.z < energyMin)
        {
            //�G�l���M�[�̍ŏ��l
            trans.localPosition = new Vector3(0, 0, energyMin);
        }

        nowRemainingEnergy = 1.0f - (energyMax - trans.localPosition.z) / allRemainingEnergy;
    }

    private IEnumerator RecoveryCoolTime()  //�񕜂܂ł̃N�[���^�C��
    {
        yield return new WaitForSeconds(coolTime);  //�N�[���^�C�����҂�

        recoveryFlg = true;
    }

    public float GetSetNowAmount
    {
        get { return nowRemainingEnergy; }
        set { useEnergy = value; }
    }
}
