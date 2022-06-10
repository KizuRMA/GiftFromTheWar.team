using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireGun : ShootParent
{
    //弾の発射
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GetItem getItem;
    private List<GameObject> explosionEffectList = new List<GameObject>();   //弾の配列
    [SerializeField] private bulletChange bulletChange;
    private Vector3 explosionPos;   //爆発位置
    private bool shotFlg;   //発射可能

    private void Start()
    {
        trans = transform;
        //explosionEffect.SetActive(false);
    }

    void Update()
    {
        if (!getItem.fireAmmunitionFlg) return; //弾を拾ってなかったら処理しない

        MoveBullet();

        if (bulletChange.nowBulletType != bulletChange.bulletType.e_fire) return;   //今の弾の種類が対応してなかったら

        BulletVecter();
        //エネルギーが必要量あれば
        shotFlg = energyAmount.GetSetNowAmount > (1.0f - useEnergy);

        //発射キーを押したら
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!shotFlg) return;
            Shot();
        }
        else
        {
            energyAmount.GetSetNowAmount = 0;
        }

        //爆発エフェクト終了処理
        if (explosionEffectList.Count != 0)
        {
            if (explosionEffectList[0].transform.childCount == 0)
                explosionEffectList.RemoveAt(0);
        }

        //弾がある状態で、右クリックすると
        if (bullet.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Explosion();
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

    private void Explosion()    //爆発処理
    {
        explosionPos = bullet[0].transform.position;
        Destroy(bullet[0]);

        explosionEffectList.Add((GameObject)Instantiate(explosionEffect, explosionPos, Quaternion.identity));
        //explosionEffect.transform.position = explosionPos;
    }
}
