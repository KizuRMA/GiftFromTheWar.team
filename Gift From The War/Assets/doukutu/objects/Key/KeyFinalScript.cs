using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFinalScript : MonoBehaviour
{
    [SerializeField] BossState state;
    [SerializeField] WallCrash wall;

    //�O������ύX����ϐ�
    public bool isGetKeyFlg { get; set; }

    private void Start()
    {
        isGetKeyFlg = false;
    }

    private void Update()
    {
        //��������Ă��Ȃ��ꍇ
        if (isGetKeyFlg == false) return;

        //�{�X���N����
        state.getupFlg = true;
        wall.openFlg = true;
        Destroy(gameObject);

    }
}
