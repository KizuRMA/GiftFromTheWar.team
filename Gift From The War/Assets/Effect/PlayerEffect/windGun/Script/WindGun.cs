using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindGun : MonoBehaviour
{
    private ParticleSystem par;
    [SerializeField] private MoveWindGun wind;

    private bool effectFlg = false;

    void Start()
    {
        par = this.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!wind.effectFlg)    //���̗͂��g���Ă��Ȃ��������\��
        {
            effectFlg = false;
            par.Stop();
            return;    
        }

        if (effectFlg) return;  //���łɃG�t�F�N�g���o���Ă����珈�����Ȃ�

        effectFlg = true;
        par.Play();
    }
}
