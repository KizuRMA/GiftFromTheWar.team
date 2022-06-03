using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    private Transform trans;
    [SerializeField] private Transform camTrans;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletEffectPrefab;
    [SerializeField] private remainingAmount energyAmount;

    //弾の発射
    private List<GameObject> bullet = new List<GameObject>();   //弾の配列
    private List<GameObject> bulletEffect = new List<GameObject>();   //弾の配列
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

    private void CreateBullet() //プレハブから弾を作る
    {
        //リストに弾を追加
        bullet.Add((GameObject)Instantiate(bulletPrefab, trans.position, Quaternion.identity));

        //目的地に球を方向転換
        bullet[bullet.Count - 1].transform.LookAt(shotPos);

        //射撃されてから指定秒後に銃弾のオブジェクトを破壊する
        Destroy(bullet[bullet.Count - 1], range);


        //リストに弾を追加
        bulletEffect.Add((GameObject)Instantiate(bulletEffectPrefab, trans.position, Quaternion.identity));

        //目的地に球を方向転換
        bulletEffect[bulletEffect.Count - 1].transform.LookAt(shotPos);

        //射撃されてから指定秒後に銃弾のオブジェクトを破壊する
        Destroy(bulletEffect[bulletEffect.Count - 1], range);
    }

    private void MoveBullet()   //弾の移動
    {
        if (bullet == null) return; //弾がなければ処理しない

        for (int i = 0; i < bullet.Count; i++) //弾の数だけ繰り返す
        {
            if (bullet[i] == null)   //弾が破壊されていたら、リストから削除
            {
                bullet.RemoveAt(i);
                bulletEffect.RemoveAt(i);
                continue;
            }
            bullet[i].transform.transform.position += bullet[i].transform.forward * shotSpeed * Time.deltaTime; //移動処理
            bulletEffect[i].transform.transform.position += bulletEffect[i].transform.forward * shotSpeed * Time.deltaTime; //移動処理

        }
    }
}
