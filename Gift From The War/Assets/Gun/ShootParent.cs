using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootParent : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    protected Transform trans;
    [SerializeField] protected Transform camTrans;
    [SerializeField] protected remainingAmount energyAmount;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected GameObject bulletEffectPrefab;
    [SerializeField] protected GameObject bulletRemainEffectPrefab;

    //弾の発射  
    [SerializeField] protected float shotSpeed;   //発射スピード
    [SerializeField] protected float range;       //弾の消えるまでの時間
    [SerializeField] protected float useEnergy;   //消費エネルギー

    private List<GameObject> bullet = new List<GameObject>();   //弾の配列
    private List<GameObject> bulletEffect = new List<GameObject>();   //弾のエフェクト
    private List<GameObject> bulletRemainEffect = new List<GameObject>();   //弾の残留エフェクト
    protected Vector3 shotPos;                    //着弾点

    protected void CreateBullet() //プレハブから弾を作る
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


        //リストに弾を追加
        bulletRemainEffect.Add((GameObject)Instantiate(bulletRemainEffectPrefab, trans.position, Quaternion.identity));

        //目的地に球を方向転換
        bulletRemainEffect[bulletRemainEffect.Count - 1].transform.LookAt(shotPos);

        //射撃されてから指定秒後に銃弾のオブジェクトを破壊する
        Destroy(bulletRemainEffect[bulletRemainEffect.Count - 1], range);
    }

    protected void MoveBullet()   //弾の移動
    {
        if (bullet == null) return; //弾がなければ処理しない

        for (int i = 0; i < bullet.Count; i++) //弾の数だけ繰り返す
        {
            if (bullet[i] == null)   //弾が破壊されていたら、リストから削除
            {
                Destroy(bulletEffect[i]);
                bullet.RemoveAt(i);
                bulletEffect.RemoveAt(i);
                bulletRemainEffect.RemoveAt(i);
                continue;
            }
            bullet[i].transform.transform.position += bullet[i].transform.forward * shotSpeed * Time.deltaTime; //移動処理
            bulletEffect[i].transform.transform.position += bulletEffect[i].transform.forward * shotSpeed * Time.deltaTime; //移動処理
            bulletRemainEffect[i].transform.transform.position += bulletRemainEffect[i].transform.forward * shotSpeed * Time.deltaTime; //移動処理
        }
    }
}
