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
    [SerializeField] private int bulletMax;     //�e�̍ő吔
    [SerializeField] private float useEnergy;   //����G�l���M�[
    private int shotCount;                      //�ł����e��
    private float shotInterval;                 //�C���^�[�o��

    private void Start()
    {
        shotCount = 0;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            energyAmount.GetSetNowAmount = 0;

            shotInterval += 1;

            if (shotInterval % 5 == 0 && shotCount > 0) //���ˏ���
            {
                shotCount -= 1;

                energyAmount.GetSetNowAmount = useEnergy;

                //�v���n�u����e�����A�e�̌����Ă�������ɔ���
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                bulletRb.AddForce(transform.forward * shotSpeed);

                //�ˌ�����Ă���3�b��ɏe�e�̃I�u�W�F�N�g��j�󂷂�.
                Destroy(bullet, 3.0f);
            }

        }
        else/* if (Input.GetKeyDown(KeyCode.R))   //�����[�h*/
        {
            if (!energyAmount.energyMaxFlg) return;

            shotCount = bulletMax;
        }

    }
}
