using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFinalScript : MonoBehaviour
{
    [SerializeField] BossState state;
    [SerializeField] WallCrash wall;
    [SerializeField] GameObject bigRock;

    //�O������ύX����ϐ�
    public bool isGetKeyFlg { get; set; }

    private void Start()
    {
        bigRock.SetActive(false);
        isGetKeyFlg = false;
    }

    private void Update()
    {
        //��������Ă��Ȃ��ꍇ
        if (isGetKeyFlg == false) return;

        KeyTakeFunction();
        Destroy(gameObject);
    }

    public void KeyTakeFunction()
    {
        state.getupFlg = true;
        wall.openFlg = true;
        bigRock.SetActive(true);
    }
}
