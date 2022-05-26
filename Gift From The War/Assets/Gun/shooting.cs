using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class shooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shotSpeed;   //���˃X�s�[�h
    [SerializeField] private int bulletMax;     //�e�̍ő吔
    private int shotCount;                      //�ł����e��
    private float shotInterval;                 //�C���^�[�o��

    private void Start()
    {
        shotCount = bulletMax;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            shotInterval += 1;

            if (shotInterval % 5 == 0 && shotCount > 0) //���ˏ���
            {
                shotCount -= 1;

                //�v���n�u����e�����A�e�̌����Ă�������ɔ���
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                bulletRb.AddForce(transform.forward * shotSpeed * Time.deltaTime);

                //�ˌ�����Ă���3�b��ɏe�e�̃I�u�W�F�N�g��j�󂷂�.
                Destroy(bullet, 3.0f);
            }

        }
        else if (Input.GetKeyDown(KeyCode.R))   //�����[�h
        {
            shotCount = bulletMax;
        }

    }
}
