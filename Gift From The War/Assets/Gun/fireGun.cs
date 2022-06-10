using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireGun : ShootParent
{
    //�e�̔���
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GetItem getItem;
    private List<GameObject> explosionEffectList = new List<GameObject>();   //�e�̔z��
    [SerializeField] private bulletChange bulletChange;
    private Vector3 explosionPos;   //�����ʒu
    private bool shotFlg;   //���ˉ\

    private void Start()
    {
        trans = transform;
        //explosionEffect.SetActive(false);
    }

    void Update()
    {
        if (!getItem.fireAmmunitionFlg) return; //�e���E���ĂȂ������珈�����Ȃ�

        MoveBullet();

        if (bulletChange.nowBulletType != bulletChange.bulletType.e_fire) return;   //���̒e�̎�ނ��Ή����ĂȂ�������

        BulletVecter();
        //�G�l���M�[���K�v�ʂ����
        shotFlg = energyAmount.GetSetNowAmount > (1.0f - useEnergy);

        //���˃L�[����������
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!shotFlg) return;
            Shot();
        }
        else
        {
            energyAmount.GetSetNowAmount = 0;
        }

        //�����G�t�F�N�g�I������
        if (explosionEffectList.Count != 0)
        {
            if (explosionEffectList[0].transform.childCount == 0)
                explosionEffectList.RemoveAt(0);
        }

        //�e�������ԂŁA�E�N���b�N�����
        if (bullet.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Explosion();
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

    private void Explosion()    //��������
    {
        explosionPos = bullet[0].transform.position;
        Destroy(bullet[0]);

        explosionEffectList.Add((GameObject)Instantiate(explosionEffect, explosionPos, Quaternion.identity));
        //explosionEffect.transform.position = explosionPos;
    }
}
