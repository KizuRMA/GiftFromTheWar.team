using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyManager : MonoBehaviour
{
    protected int numberEnemies;

    public void EnemyCounter()
    {
        numberEnemies += 1;
    }

    protected virtual void EnemyReSpawn()   //�G�����X�|�[�������钊�ۉ��֐�
    {

    }
}
