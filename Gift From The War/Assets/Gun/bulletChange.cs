using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletChange : MonoBehaviour
{
    //�X�N���v�g�擾
    [SerializeField] private remainingAmount RA;
    [SerializeField] private MoveWindGun moveWind;
    [SerializeField] public Cylinder cylinder;
    [SerializeField] public GetItem getItem;

    private shooting shooting;
    private magnet magnet;
    private magnetChain magnetChain;
    private fireGun fireGun;

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

    public bool IsHaveBullet    //�e�������Ă��邩
    {
        get
        {
            return (getItem.windAmmunitionFlg || getItem.magnetAmmunitionFlg || getItem.fireAmmunitionFlg);
        }
    }

    void Start()
    {
        Transform _muzzleTrans = transform.Find("muzzlePos");
        shooting = _muzzleTrans.GetComponent<shooting>();
        magnet = _muzzleTrans.GetComponent<magnet>();
        fireGun = _muzzleTrans.GetComponent<fireGun>();
        magnetChain = _muzzleTrans.GetComponent<magnetChain>();

        nowBulletType = bulletType.e_wind;
    }

    void Update()
    {
        //if (!RA.energyMaxFlg) return;   //�G�l���M�[������Ă�����A�؂�ւ��ł��Ȃ�
        if (!changeableFlg) return;

        WheelBulletChange();
        ControllBulletScript();
    }

    private IEnumerator RecoveryCoolTime()  //�񕜂܂ł̃N�[���^�C��
    {
        yield return new WaitForSeconds(coolTime);  //�N�[���^�C�����҂�

        changeableFlg = true;
    }

    private void WheelBulletChange()    //�e�ύX�̏���
    {
        //�ǂ̒e�������Ă��Ȃ���
        if (IsHaveBullet == false) return;

        //��]�̎擾
        wheel = Input.GetAxis("Mouse ScrollWheel");

        if (wheel == 0) return; //�X�N���[�����Ȃ������珈�����Ȃ�

        changeableFlg = false;
        scriptChangeFlg = true;
        AudioManager.Instance.PlaySE("Clinder1", isLoop: false);

        BulletChange();

        StartCoroutine(RecoveryCoolTime());
    }

    private void ControllBulletScript() //�X�N���v�g�̃I���I�t
    {
        if (!scriptChangeFlg || cylinder.isChanging) return;

        scriptChangeFlg = false;

        //�e���ς������A�e�̏�����񃊃Z�b�g����
        moveWind.Finish();
        magnet.Finish();
        magnetChain.Finish();
    }

    private void BulletChange()
    {
        //�ǂ����ɃX�N���[��������
        int _changeNum = wheel > 0 ? 1 : -1;
        int _bulletTypeMax = System.Enum.GetValues(typeof(bulletType)).Length;
        cylinder.isChanging = true;

        while (true)
        {
            //�g�p����e��ύX
            nowBulletType = (bulletType)System.Enum.ToObject(typeof(bulletType), (((int)nowBulletType) + _changeNum + _bulletTypeMax) % _bulletTypeMax);

            switch (nowBulletType)
            {
                case bulletType.e_wind:
                    if (!getItem.windAmmunitionFlg)continue;
                    break;
                case bulletType.e_magnet:
                    if (!getItem.magnetAmmunitionFlg)continue;
                    break;
                case bulletType.e_fire:
                    if (!getItem.fireAmmunitionFlg)continue;
                    break;
            }
            break;
        }
    }

    public void HaveBulletAutoChange()  //�����Ă���e�Ɏ����I�ɐ؂�ւ���
    {
        if (IsHaveBullet == false) return;

        //�ǂ����ɃX�N���[��������
        int _changeNum = 1;
        int _bulletTypeMax = System.Enum.GetValues(typeof(bulletType)).Length;

        while (true)
        {
            //�g�p����e��ύX
            nowBulletType = (bulletType)System.Enum.ToObject(typeof(bulletType), (((int)nowBulletType) + _changeNum + _bulletTypeMax) % _bulletTypeMax);

            switch (nowBulletType)
            {
                case bulletType.e_wind:
                    if (!getItem.windAmmunitionFlg) continue;
                    break;
                case bulletType.e_magnet:
                    if (!getItem.magnetAmmunitionFlg) continue;
                    break;
                case bulletType.e_fire:
                    if (!getItem.fireAmmunitionFlg) continue;
                    break;
            }
            break;
        }
    }
}
