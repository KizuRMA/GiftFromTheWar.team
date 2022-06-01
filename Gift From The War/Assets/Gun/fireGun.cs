using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireGun : MonoBehaviour
{
    ///ゲームオブジェクトやスクリプト
    private Transform trans;
    [SerializeField] private Transform camTrans;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private remainingAmount energyAmount;

    //弾の発射
    [SerializeField] private float shotSpeed;   //発射スピード
    [SerializeField] private float range;       //弾の消えるまでの時間
    [SerializeField] private float useEnergy;   //消費エネルギー
    private bool shotFlg;                       //発射可能
    private Quaternion bulletQua;               //発射する弾の向き
    private Vector3 shotPos;                    //着弾点

    private void Start()
    {
        trans = transform;
    }

    void Update()
    {
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

    private void CreateBullet() //プレハブから弾を作る
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, trans.position, Quaternion.identity);
        trans.LookAt(shotPos);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(trans.forward * shotSpeed * Time.deltaTime);

        //射撃されてから指定秒後に銃弾のオブジェクトを破壊する
        Destroy(bullet, range);
    }
}
