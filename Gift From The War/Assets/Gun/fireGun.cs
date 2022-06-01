using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireGun : MonoBehaviour
{
    ///�Q�[���I�u�W�F�N�g��X�N���v�g
    private Transform trans;
    [SerializeField] private Transform camTrans;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private remainingAmount energyAmount;

    //�e�̔���
    [SerializeField] private float shotSpeed;   //���˃X�s�[�h
    [SerializeField] private float range;       //�e�̏�����܂ł̎���
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
        //�G�l���M�[���K�v�ʂ����
        shotFlg = energyAmount.GetSetNowAmount > (1.0f - useEnergy);

        //���˃L�[����������
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!shotFlg) return;
            Shot();
        }
        else
        {
            energyAmount.GetSetNowAmount = 0;
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
        bulletRb.AddForce(trans.forward * shotSpeed * Time.deltaTime);

        //�ˌ�����Ă���w��b��ɏe�e�̃I�u�W�F�N�g��j�󂷂�
        Destroy(bullet, range);
    }
}
