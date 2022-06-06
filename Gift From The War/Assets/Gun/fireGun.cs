using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireGun : ShootParent
{
    //弾の発射
    [SerializeField] private bulletChange bulletChange;
    private bool shotFlg;                       //発射可能

    private void Start()
    {
        trans = transform;
    }

    void Update()
    {
        MoveBullet();

        if (bulletChange.nowBulletType != bulletChange.bulletType.e_fire) return;   //今の弾の種類が対応してなかったら

        BulletVecter();
        //エネルギーが必要量あれば
        shotFlg = energyAmount.GetSetNowAmount > (1.0f - useEnergy);

        //発射キーを押したら
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!shotFlg) return;
            Shot();
        }
        else
        {
            energyAmount.GetSetNowAmount = 0;
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
