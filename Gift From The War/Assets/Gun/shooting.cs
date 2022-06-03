using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    private Transform trans;
    [SerializeField] private Transform camTrans;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletEffectPrefab;
    [SerializeField] private remainingAmount energyAmount;

    //�e�̔���
    private List<GameObject> bullet = new List<GameObject>();   //�e�̔z��
    private List<GameObject> bulletEffect = new List<GameObject>();   //�e�̔z��
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

        MoveBullet();

        //���˃L�[����������
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
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
        //���X�g�ɒe��ǉ�
        bullet.Add((GameObject)Instantiate(bulletPrefab, trans.position, Quaternion.identity));

        //�ړI�n�ɋ�������]��
        bullet[bullet.Count - 1].transform.LookAt(shotPos);

        //�ˌ�����Ă���w��b��ɏe�e�̃I�u�W�F�N�g��j�󂷂�
        Destroy(bullet[bullet.Count - 1], range);


        //���X�g�ɒe��ǉ�
        bulletEffect.Add((GameObject)Instantiate(bulletEffectPrefab, trans.position, Quaternion.identity));

        //�ړI�n�ɋ�������]��
        bulletEffect[bulletEffect.Count - 1].transform.LookAt(shotPos);

        //�ˌ�����Ă���w��b��ɏe�e�̃I�u�W�F�N�g��j�󂷂�
        Destroy(bulletEffect[bulletEffect.Count - 1], range);
    }

    private void MoveBullet()   //�e�̈ړ�
    {
        if (bullet == null) return; //�e���Ȃ���Ώ������Ȃ�

        for (int i = 0; i < bullet.Count; i++) //�e�̐������J��Ԃ�
        {
            if (bullet[i] == null)   //�e���j�󂳂�Ă�����A���X�g����폜
            {
                bullet.RemoveAt(i);
                bulletEffect.RemoveAt(i);
                continue;
            }
            bullet[i].transform.transform.position += bullet[i].transform.forward * shotSpeed * Time.deltaTime; //�ړ�����
            bulletEffect[i].transform.transform.position += bulletEffect[i].transform.forward * shotSpeed * Time.deltaTime; //�ړ�����

        }
    }
}
