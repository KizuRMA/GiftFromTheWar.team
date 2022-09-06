using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFinalScript : MonoBehaviour
{
    [SerializeField] BossState state;
    [SerializeField] WallCrash wall;

    //外部から変更する変数
    public bool isGetKeyFlg { get; set; }

    private void Start()
    {
        isGetKeyFlg = false;
    }

    private void Update()
    {
        //鍵が取られていない場合
        if (isGetKeyFlg == false) return;

        //ボスを起こす
        state.getupFlg = true;
        wall.openFlg = true;
        Destroy(gameObject);

    }
}
