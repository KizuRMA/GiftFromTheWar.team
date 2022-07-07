using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyManager : MonoBehaviour
{
    protected int numberEnemies;    //�G�̐�
    protected int numberRespawnPlan;  //���X�|�[������\��̐�

    private void Awake()
    {
        numberEnemies = 0;
        numberRespawnPlan = 0;
    }

    public void EnemyCounter()
    {
        numberEnemies += 1;
    }

    public void RespawnPlanCounter(int _num)
    {
        numberRespawnPlan += _num;
        numberRespawnPlan = Mathf.Max(numberRespawnPlan, 0);
    }

    protected virtual void EnemyReSpawn()   //�G�����X�|�[�������钊�ۉ��֐�
    {

    }

    public bool IsEnemysChasing()
    {
        return IsChasing();
    }

    protected virtual bool IsChasing()
    {
        return false;
    }

    public GameObject[] GetEnemys()
    {
        return GetChildObjects();
    }

    protected GameObject[] GetChildObjects()
    {
        //�q�I�u�W�F�N�g��S�Ď擾����
        GameObject[] _ChildObjects = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _ChildObjects[i] = transform.GetChild(i).gameObject;
        }

        return _ChildObjects;
    }
}
