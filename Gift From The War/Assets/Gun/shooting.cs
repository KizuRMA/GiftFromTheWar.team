using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : ShootParent
{
    //�e�̔���  
    [SerializeField] private GetItem getItem;
    [SerializeField] private bulletChange bulletChange;
    private bool shotableFlg;   //���ˉ\
    public bool  shotFlg;       //���˂���

    private void Start()
    {
        trans = transform;
    }

    void Update()
    {
        if (!getItem.windAmmunitionFlg) return; //�e���E���ĂȂ������珈�����Ȃ�

        MoveBullet();

        if (bulletChange.nowBulletType != bulletChange.bulletType.e_wind) return;   //���̒e�̎�ނ��Ή����ĂȂ�������

        BulletVecter();
        //�G�l���M�[���K�v�ʂ����
        shotableFlg = energyAmount.GetSetNowAmount > (1.0f - useEnergy);

        shotFlg = false;

        //���˃L�[����������
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!shotableFlg) return;
            shotFlg = true;
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
}
