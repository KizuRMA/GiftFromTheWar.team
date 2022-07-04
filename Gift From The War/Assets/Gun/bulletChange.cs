using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletChange : MonoBehaviour
{
    //スクリプト取得
    [SerializeField] private remainingAmount RA;
    [SerializeField] private MoveWindGun moveWind;
    private shooting shooting;
    private magnet magnet;
    private magnetChain magnetChain;
    private fireGun fireGun;

    //武器切り替え
    public enum bulletType
    {
        e_wind,
        e_magnet,
        e_fire
    }
    private float wheel;    //ホイールのスクロール量
    public bulletType nowBulletType { get; set; } //今の武器

    //クールタイム
    [SerializeField] private float coolTime;    //エネルギー回復のクールタイム
    private bool changeableFlg = true;          //チェンジ可能かどうか

    //スクリプト
    private bool scriptChangeFlg = true;   //スクリプト切り替えようのフラグ

    void Start()
    {
        Transform _muzzleTrans = transform.Find("muzzlePos");
        shooting = _muzzleTrans.GetComponent<shooting>();
        magnet = _muzzleTrans.GetComponent<magnet>();
        fireGun = _muzzleTrans.GetComponent<fireGun>();
        magnetChain = _muzzleTrans.GetComponent<magnetChain>();

        nowBulletType = bulletType.e_wind;
    }

    void Update()
    {
        //if (!RA.energyMaxFlg) return;   //エネルギーを消費していたら、切り替えできない
        if (!changeableFlg) return;

        wheelBulletChange();
        ControllBulletScript();
    }

    private IEnumerator RecoveryCoolTime()  //回復までのクールタイム
    {
        yield return new WaitForSeconds(coolTime);  //クールタイム分待つ

        changeableFlg = true;
    }

    private void wheelBulletChange()    //弾変更の処理
    {
        //回転の取得
        wheel = Input.GetAxis("Mouse ScrollWheel");

        if (wheel == 0) return; //スクロールしなかったら処理しない

        changeableFlg = false;
        scriptChangeFlg = true;

        //どっちにスクロールしたか
        nowBulletType += wheel > 0 ? 1 : -1;

        //最大値最小値を超えると、一周回る
        if (nowBulletType > bulletType.e_fire) nowBulletType = bulletType.e_wind;
        if (nowBulletType < bulletType.e_wind) nowBulletType = bulletType.e_fire;

        StartCoroutine(RecoveryCoolTime());
    }

    private void ControllBulletScript() //スクリプトのオンオフ
    {
        if (!scriptChangeFlg) return;

        scriptChangeFlg = false;

        //弾が変わったら、弾の情報を一回リセットする
        moveWind.Finish();
        magnet.Finish();
        magnetChain.Finish();
    }
}
