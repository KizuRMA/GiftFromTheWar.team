using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class shooting : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private remainingAmount energyAmount;

    //弾の発射
    [SerializeField] private float shotSpeed;   //発射スピード
    [SerializeField] private int bulletMax;     //弾の最大数
    [SerializeField] private float useEnergy;   //消費エネルギー
    private int shotCount;                      //打った弾数
    private float shotInterval;                 //インターバル

    private void Start()
    {
        shotCount = 0;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            energyAmount.GetSetNowAmount = 0;

            shotInterval += 1;

            if (shotInterval % 5 == 0 && shotCount > 0) //発射処理
            {
                shotCount -= 1;

                energyAmount.GetSetNowAmount = useEnergy;

                //プレハブから弾を作り、銃の向いている向きに発射
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                bulletRb.AddForce(transform.forward * shotSpeed);

                //射撃されてから3秒後に銃弾のオブジェクトを破壊する.
                Destroy(bullet, 3.0f);
            }

        }
        else/* if (Input.GetKeyDown(KeyCode.R))   //リロード*/
        {
            if (!energyAmount.energyMaxFlg) return;

            shotCount = bulletMax;
        }

    }
}
