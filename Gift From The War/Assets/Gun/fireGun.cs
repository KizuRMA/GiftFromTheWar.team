using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireGun : ShootParent
{
    //�e�̔���
    [SerializeField] private bulletChange bulletChange;
    private bool shotFlg;                       //���ˉ\

    private void Start()
    {
        trans = transform;
    }

    void Update()
    {
        MoveBullet();

        if (bulletChange.nowBulletType != bulletChange.bulletType.e_fire) return;   //���̒e�̎�ނ��Ή����ĂȂ�������

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
}
