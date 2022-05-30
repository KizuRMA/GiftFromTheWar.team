using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    private Transform trans;
    [SerializeField] private Transform camTrans;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private remainingAmount energyAmount;

    //�e�̔���
    [SerializeField] private float shotSpeed;   //���˃X�s�[�h
    [SerializeField] private float useEnergy;   //����G�l���M�[
    private bool shotFlg;                       //���ˉ\
    private Quaternion bulletQua;               //���˂���e�̌���
    private Vector3 shotPos;                    //���e�_

    private void Start()
    {
        trans = transform;
    }

    void Update()
    {
        BulletVecter();
        //�G�l���M�[���ő�܂ł��܂��Ă�����A���˂ł���
        shotFlg = energyAmount.energyMaxFlg;

        //���˃L�[����������
        if (Input.GetKey(KeyCode.Mouse1))
        {
            energyAmount.GetSetNowAmount = 0;

            if (!shotFlg) return;
            Shot();
        }
    }

    private void Shot() //�e��ł���
    {
        energyAmount.GetSetNowAmount = useEnergy;
        energyAmount.useDeltaTime = false;

        BulletVecter();

        CreateBullet();
    }

    private void BulletVecter() //�e�̌��������߂�
    {
        //�v���C���[�̑O�Ƀ��C������΂��A�I�u�W�F�N�g�Ƃ̋��������߂�B
        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            shotPos = hit.point;
        }
    }

    private void CreateBullet() //�v���n�u����e�����
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, trans.position, Quaternion.identity);
        trans.LookAt(shotPos);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(trans.forward * shotSpeed);

        //�ˌ�����Ă���3�b��ɏe�e�̃I�u�W�F�N�g��j�󂷂�.
        Destroy(bullet, 3.0f);
    }
}
