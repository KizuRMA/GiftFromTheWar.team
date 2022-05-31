using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletChange : MonoBehaviour
{
    //�X�N���v�g�擾
    [SerializeField] private remainingAmount RA;
    [SerializeField] private MoveWindGun moveWind;
    [SerializeField] private shooting shooting;
    [SerializeField] private magnet magnet;

    //����؂�ւ�
    public enum bulletType
    {
        e_wind,
        e_magnet,
        e_fire
    }
    private float wheel;    //�z�C�[���̃X�N���[����
    public bulletType nowBulletType { get; set; } //���̕��� 

    //�N�[���^�C��
    [SerializeField] private float coolTime;    //�G�l���M�[�񕜂̃N�[���^�C��
    private bool changeableFlg = true;          //�`�F���W�\���ǂ���

    //�X�N���v�g
    private bool scriptChangeFlg = true;   //�X�N���v�g�؂�ւ��悤�̃t���O     

    void Start()
    {
        nowBulletType = bulletType.e_wind;
    }

    void Update()
    {
        if (!RA.energyMaxFlg) return;   //�G�l���M�[������Ă�����A�؂�ւ��ł��Ȃ�
        if (!changeableFlg) return;

        wheelBulletChange();
        ControllBulletScript();
    }

    private IEnumerator RecoveryCoolTime()  //�񕜂܂ł̃N�[���^�C��
    {
        yield return new WaitForSeconds(coolTime);  //�N�[���^�C�����҂�

        changeableFlg = true;
    }

    private void wheelBulletChange()    //�e�ύX�̏���
    {
        //��]�̎擾
        wheel = Input.GetAxis("Mouse ScrollWheel");

        if (wheel == 0) return; //�X�N���[�����Ȃ������珈�����Ȃ�

        changeableFlg = false;
        scriptChangeFlg = true;

        //�ǂ����ɃX�N���[��������
        nowBulletType += wheel > 0 ? 1 : -1;

        //�ő�l�ŏ��l�𒴂���ƁA������
        if (nowBulletType > bulletType.e_fire) nowBulletType = bulletType.e_wind;
        if (nowBulletType < bulletType.e_wind) nowBulletType = bulletType.e_fire;

        StartCoroutine(RecoveryCoolTime());
    }

    private void ControllBulletScript() //�X�N���v�g�̃I���I�t
    {
        if (!scriptChangeFlg) return;

        scriptChangeFlg = false;

        //�e�̎�ނ킯
        switch (nowBulletType)
        {
            case bulletChange.bulletType.e_wind:
                moveWind.enabled = true;
                shooting.enabled = true;
                magnet.enabled = false;
                break;

            case bulletChange.bulletType.e_magnet:
                moveWind.enabled = false;
                shooting.enabled = false;
                magnet.enabled = true;
                break;

            case bulletChange.bulletType.e_fire:
                moveWind.enabled = false;
                shooting.enabled = false;
                magnet.enabled = false;
                break;
        }
    }
}
