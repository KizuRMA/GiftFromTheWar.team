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
    private float nowRemainingAmount;           //���̎c��
    private float useAmount;                    //�����
    private float allRemainingAmount;           //�v�Z�ɕK�v�Ȓ萔
    [SerializeField] private float upSpeed;     //�񕜃X�s�[�h
    [SerializeField] private float amountMax;   //�ő��
    [SerializeField] private float amountMin;   //�ŏ���

    void Start()
    {
        firstPos = transform.position;
        allRemainingAmount = amountMax - amountMin;
    }

    void Update()
    {
        if (useAmount == 0) //�G�l���M�[����Ȃ�������
        {
            //�G�l���M�[�̉�
            trans.localPosition += new Vector3(0, 0, -(upSpeed * allRemainingAmount - allRemainingAmount) * Time.deltaTime);
        }
        else
        {
            //�G�l���M�[�̏����
            trans.localPosition += new Vector3(0, 0, (useAmount * allRemainingAmount - allRemainingAmount) * Time.deltaTime);

            //�G�l���M�[�̐F�ύX
            Material[] tmp = gameObject.GetComponent<Renderer>().materials;
            tmp[0] = mat1;
            amount2.GetComponent<Renderer>().materials = tmp;
        }

        if (trans.localPosition.z > amountMax)
        {
            //�G�l���M�[�̍ő�l
            trans.localPosition = new Vector3(0, 0, amountMax);

            //�G�l���M�[�̐F�ύX
            Material[] tmp = gameObject.GetComponent<Renderer>().materials;
            tmp[0] = mat2;
            amount2.GetComponent<Renderer>().materials = tmp;
        }
        else if (trans.localPosition.z < amountMin)
        {
            //�G�l���M�[�̍ŏ��l
            trans.localPosition = new Vector3(0, 0, amountMin);
        }

        nowRemainingAmount = 1.0f - (amountMax - trans.localPosition.z) / allRemainingAmount;
    }

    public float GetSetNowAmount
    {
        get { return nowRemainingAmount; }
        set { useAmount = value; }
    }
}
