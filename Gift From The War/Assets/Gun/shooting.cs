using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : ShootParent
{
    //弾の発射  
    [SerializeField] private GetItem getItem;
    [SerializeField] private bulletChange bulletChange;
    private bool shotableFlg;   //発射可能
    public bool  shotFlg;       //発射した

    private void Start()
    {
        trans = transform;
    }

    void Update()
    {
        if (!getItem.windAmmunitionFlg) return; //弾を拾ってなかったら処理しない

        MoveBullet();

        if (bulletChange.nowBulletType != bulletChange.bulletType.e_wind) return;   //今の弾の種類が対応してなかったら

        BulletVecter();
        //エネルギーが必要量あれば
        shotableFlg = energyAmount.GetSetNowAmount > (1.0f - useEnergy);

        shotFlg = false;

        //発射キーを押したら
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!shotableFlg) return;
            shotFlg = true;
            Shot();
        }
    }

    private void Shot() //弾を打つ処理
    {
        energyAmount.GetSetNowAmount = useEnergy;
        energyAmount.useDeltaTime = false;

        BulletVecter();

        CreateBullet();
    }

    private void BulletVecter() //弾の向きを決める
    {
        //プレイヤーの前にレイ判定を飛ばし、オブジェクトとの距離を求める。
        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            shotPos = hit.point;
        }
    }
}
