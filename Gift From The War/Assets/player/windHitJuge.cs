using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windHitJuge : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    private Transform trans;
    private MeshCollider MC;
    [SerializeField] private MoveWindGun wind;

    //風が当たった処理
    [SerializeField] private float windPower;
    [SerializeField] private float windPowerMax;
    private float nowWindPower = 0;
    [SerializeField] private float windPower2;
    [SerializeField] private float windPowerMax2;
    private float nowWindPower2 = 0;
    private Vector3 firstScale;

    void Start()
    {
        GameObject _parent = transform.parent.gameObject;
        //親オブジェクトがある場合
        if (_parent == true)
        {
            wind = _parent.GetComponent<WindGunEffectInfo>().gun;
        }

        trans = transform;
        firstScale = trans.localScale;
        MC = this.GetComponent<MeshCollider>();
    }

    void Update()
    {
        if (!wind.effectFlg)    //風を出していなかったら処理しない
        {
            if (!MC.enabled) return; //すでに当たり判定を消していたら処理しない
            nowWindPower = 0;
            nowWindPower2 = 0;
            MC.enabled = false;
            return;
        }

        if(!MC.enabled) MC.enabled = true;  //当たり判定をオンにする

        //当たり判定を長くする
        nowWindPower += windPower * Time.deltaTime;
        nowWindPower = nowWindPower > windPowerMax ? windPowerMax : nowWindPower;

        //当たり判定を大きくする
        nowWindPower2 += windPower2 * Time.deltaTime;
        nowWindPower2 = nowWindPower2 > windPowerMax2 ? windPowerMax2 : nowWindPower2;

        trans.localScale = new Vector3(nowWindPower2, nowWindPower2, nowWindPower);

    }
}
