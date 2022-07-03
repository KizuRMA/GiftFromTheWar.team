using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : ShootParent
{
    //�e�̔���  
    [SerializeField] private GetItem getItem;
    [SerializeField] private bulletChange bulletChange;
    [SerializeField] private GameObject touchEffect;
    private List<GameObject> touchEffectList = new List<GameObject>();
    private Vector3 bulletPos;  //�e�̏ꏊ��ۑ�
    private bool shotableFlg;   //���ˉ\
    public bool  shotFlg;       //���˂���
    public bool bulletTuochFlg { get; set; }    //���˂��ꂽ

    [SerializeField] private LayerMask layer;

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

        //�������������G�t�F�N�g�̏I������
        if (touchEffectList.Count != 0)
        {
            if (touchEffectList[0].transform.childCount <= 0)
            {
                Destroy(touchEffectList[0]);
                touchEffectList.RemoveAt(0);
            }
        }

        //���˃L�[����������
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!shotableFlg) return;
            shotFlg = true;
            Shot();
        }

        if (bullet.Count > 0)   //�e�̏ꏊ��ۑ����Ă���
        {
            bulletPos = bullet[0].transform.position;
        }

        if (!bulletTuochFlg) return;
        TouchBullet();
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
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            Debug.Log(hit.point);
            shotPos = hit.point;
        }
    }

    private void TouchBullet()  //�e�����̃I�u�W�F�N�g�ɓ���������
    {
        bulletTuochFlg = false;

        touchEffectList.Add((GameObject)Instantiate(touchEffect, bulletPos, Quaternion.identity));
    }
}
