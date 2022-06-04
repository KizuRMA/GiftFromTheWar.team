using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootParent : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    protected Transform trans;
    [SerializeField] protected Transform camTrans;
    [SerializeField] protected remainingAmount energyAmount;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected GameObject bulletEffectPrefab;
    [SerializeField] protected GameObject bulletRemainEffectPrefab;

    //�e�̔���  
    [SerializeField] protected float shotSpeed;   //���˃X�s�[�h
    [SerializeField] protected float range;       //�e�̏�����܂ł̎���
    [SerializeField] protected float useEnergy;   //����G�l���M�[

    private List<GameObject> bullet = new List<GameObject>();   //�e�̔z��
    private List<GameObject> bulletEffect = new List<GameObject>();   //�e�̃G�t�F�N�g
    private List<GameObject> bulletRemainEffect = new List<GameObject>();   //�e�̎c���G�t�F�N�g
    protected Vector3 shotPos;                    //���e�_

    protected void CreateBullet() //�v���n�u����e�����
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


        //���X�g�ɒe��ǉ�
        bulletRemainEffect.Add((GameObject)Instantiate(bulletRemainEffectPrefab, trans.position, Quaternion.identity));

        //�ړI�n�ɋ�������]��
        bulletRemainEffect[bulletRemainEffect.Count - 1].transform.LookAt(shotPos);

        //�ˌ�����Ă���w��b��ɏe�e�̃I�u�W�F�N�g��j�󂷂�
        Destroy(bulletRemainEffect[bulletRemainEffect.Count - 1], range);
    }

    protected void MoveBullet()   //�e�̈ړ�
    {
        if (bullet == null) return; //�e���Ȃ���Ώ������Ȃ�

        for (int i = 0; i < bullet.Count; i++) //�e�̐������J��Ԃ�
        {
            if (bullet[i] == null)   //�e���j�󂳂�Ă�����A���X�g����폜
            {
                Destroy(bulletEffect[i]);
                bullet.RemoveAt(i);
                bulletEffect.RemoveAt(i);
                bulletRemainEffect.RemoveAt(i);
                continue;
            }
            bullet[i].transform.transform.position += bullet[i].transform.forward * shotSpeed * Time.deltaTime; //�ړ�����
            bulletEffect[i].transform.transform.position += bulletEffect[i].transform.forward * shotSpeed * Time.deltaTime; //�ړ�����
            bulletRemainEffect[i].transform.transform.position += bulletRemainEffect[i].transform.forward * shotSpeed * Time.deltaTime; //�ړ�����
        }
    }
}
