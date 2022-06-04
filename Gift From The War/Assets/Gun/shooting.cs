using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : ShootParent
{
    //弾の発射  
    private bool shotFlg;                       //発射可能
    private Quaternion bulletQua;               //発射する弾の向き

    private void Start()
    {
        trans = transform;
    }

    void Update()
    {
        BulletVecter();
        //エネルギーが必要量あれば
        shotFlg = energyAmount.GetSetNowAmount > (1.0f - useEnergy);

        MoveBullet();

        //発射キーを押したら
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!shotFlg) return;
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
