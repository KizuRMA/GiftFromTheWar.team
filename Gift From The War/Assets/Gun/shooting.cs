using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : ShootParent
{
    //弾の発射  
    [SerializeField] private GetItem getItem;
    [SerializeField] private bulletChange bulletChange;
    [SerializeField] private GameObject touchEffect;
    private List<GameObject> touchEffectList = new List<GameObject>();
    private Vector3 bulletPos;  //弾の場所を保存
    private bool shotableFlg;   //発射可能
    public bool  shotFlg;       //発射した
    public bool bulletTuochFlg { get; set; }    //発射された

    [SerializeField] private LayerMask layer;

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

        //風が当たったエフェクトの終了処理
        if (touchEffectList.Count != 0)
        {
            if (touchEffectList[0].transform.childCount <= 0)
            {
                Destroy(touchEffectList[0]);
                touchEffectList.RemoveAt(0);
            }
        }

        //発射キーを押したら
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!shotableFlg) return;
            shotFlg = true;
            Shot();
        }

        if (bullet.Count > 0)   //弾の場所を保存しておく
        {
            bulletPos = bullet[0].transform.position;
        }

        if (!bulletTuochFlg) return;
        TouchBullet();
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
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            Debug.Log(hit.point);
            shotPos = hit.point;
        }
    }

    private void TouchBullet()  //弾が他のオブジェクトに当たったら
    {
        bulletTuochFlg = false;

        touchEffectList.Add((GameObject)Instantiate(touchEffect, bulletPos, Quaternion.identity));
    }
}
