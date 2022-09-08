using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFinalScript : MonoBehaviour
{
    [SerializeField] BossState state;
    [SerializeField] WallCrash wall;
    [SerializeField] GameObject bigRock;

    //ŠO•”‚©‚ç•ÏX‚·‚é•Ï”
    public bool isGetKeyFlg { get; set; }

    private void Start()
    {
        bigRock.SetActive(false);
        isGetKeyFlg = false;
    }

    private void Update()
    {
        //Œ®‚ªæ‚ç‚ê‚Ä‚¢‚È‚¢ê‡
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
