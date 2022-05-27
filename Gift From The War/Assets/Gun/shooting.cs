using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private remainingAmount energyAmount;

    //�e�̔���
    [SerializeField] private float shotSpeed;   //���˃X�s�[�h
    [SerializeField] private float useEnergy;   //����G�l���M�[
    private bool shotFlg;                       //���ˉ\
    private float shotInterval;                 //�C���^�[�o��

    private void Start()
    {
    }

    void Update()
    {
        //�G�l���M�[���ő�܂ł��܂��Ă�����A���˂ł���
        if (energyAmount.energyMaxFlg)
        {
            shotFlg = true;
        }
        else
        {
            shotFlg = false;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            energyAmount.GetSetNowAmount = 0;

            shotInterval += 1;

            if (shotFlg) //���ˏ���
            {
                energyAmount.GetSetNowAmount = useEnergy;
                energyAmount.useDeltaTime = false;

                //�v���n�u����e�����A�e�̌����Ă�������ɔ���
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                bulletRb.AddForce(transform.forward * shotSpeed);

                //�ˌ�����Ă���3�b��ɏe�e�̃I�u�W�F�N�g��j�󂷂�.
                Destroy(bullet, 3.0f);
            }

        }
    }
}
