using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFinalScript : MonoBehaviour
{
    [SerializeField] BossState state;
    [SerializeField] WallCrash wall;
    [SerializeField] GameObject bigRock;

    //外部から変更する変数
    public bool isGetKeyFlg { get; set; }

    private void Start()
    {
        bigRock.SetActive(false);
        isGetKeyFlg = false;
    }

    private void Update()
    {
        //鍵が取られていない場合
        if (isGetKeyFlg == false) return;

        //ボスを起こす
        state.getupFlg = true;
        wall.openFlg = true;
        bigRock.SetActive(true);
        Destroy(gameObject);

    }
}
