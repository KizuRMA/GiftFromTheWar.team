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
    [SerializeField] private float useEnergy;   //消費エネルギー
    private bool shotFlg;                       //発射可能
    private float shotInterval;                 //インターバル

    private void Start()
    {
    }

    void Update()
    {
        //エネルギーが最大までたまっていたら、発射できる
        if (energyAmount.energyMaxFlg)
        {
            shotFlg = true;
        }
        else
        {
            shotFlg = false;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            energyAmount.GetSetNowAmount = 0;

            shotInterval += 1;

            if (shotFlg) //発射処理
            {
                energyAmount.GetSetNowAmount = useEnergy;
                energyAmount.useDeltaTime = false;

                //プレハブから弾を作り、銃の向いている向きに発射
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                bulletRb.AddForce(transform.forward * shotSpeed);

                //射撃されてから3秒後に銃弾のオブジェクトを破壊する.
                Destroy(bullet, 3.0f);
            }

        }
    }
}
