using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletChange : MonoBehaviour
{
    //スクリプト取得
    [SerializeField] private remainingAmount RA;
    [SerializeField] private MoveWindGun moveWind;
    [SerializeField] public Cylinder cylinder;
    [SerializeField] public GetItem getItem;

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

    public bool IsHaveBullet    //弾を持っているか
    {
        get
        {
            return (getItem.windAmmunitionFlg || getItem.magnetAmmunitionFlg || getItem.fireAmmunitionFlg);
        }
    }

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

        WheelBulletChange();
        ControllBulletScript();
    }

    private IEnumerator RecoveryCoolTime()  //回復までのクールタイム
    {
        yield return new WaitForSeconds(coolTime);  //クールタイム分待つ

        changeableFlg = true;
    }

    private void WheelBulletChange()    //弾変更の処理
    {
        //どの弾も持っていない時
        if (IsHaveBullet == false) return;

        //回転の取得
        wheel = Input.GetAxis("Mouse ScrollWheel");

        if (wheel == 0) return; //スクロールしなかったら処理しない

        changeableFlg = false;
        scriptChangeFlg = true;
        AudioManager.Instance.PlaySE("Clinder1", isLoop: false);

        BulletChange();

        StartCoroutine(RecoveryCoolTime());
    }

    private void ControllBulletScript() //スクリプトのオンオフ
    {
        if (!scriptChangeFlg || cylinder.isChanging) return;

        scriptChangeFlg = false;

        //弾が変わったら、弾の情報を一回リセットする
        moveWind.Finish();
        magnet.Finish();
        magnetChain.Finish();
    }

    private void BulletChange()
    {
        //どっちにスクロールしたか
        int _changeNum = wheel > 0 ? 1 : -1;
        int _bulletTypeMax = System.Enum.GetValues(typeof(bulletType)).Length;
        cylinder.isChanging = true;

        while (true)
        {
            //使用する弾を変更
            nowBulletType = (bulletType)System.Enum.ToObject(typeof(bulletType), (((int)nowBulletType) + _changeNum + _bulletTypeMax) % _bulletTypeMax);

            switch (nowBulletType)
            {
                case bulletType.e_wind:
                    if (!getItem.windAmmunitionFlg)continue;
                    break;
                case bulletType.e_magnet:
                    if (!getItem.magnetAmmunitionFlg)continue;
                    break;
                case bulletType.e_fire:
                    if (!getItem.fireAmmunitionFlg)continue;
                    break;
            }
            break;
        }
    }

    public void HaveBulletAutoChange()  //持っている弾に自動的に切り替える
    {
        if (IsHaveBullet == false) return;

        //どっちにスクロールしたか
        int _changeNum = 1;
        int _bulletTypeMax = System.Enum.GetValues(typeof(bulletType)).Length;

        while (true)
        {
            //使用する弾を変更
            nowBulletType = (bulletType)System.Enum.ToObject(typeof(bulletType), (((int)nowBulletType) + _changeNum + _bulletTypeMax) % _bulletTypeMax);

            switch (nowBulletType)
            {
                case bulletType.e_wind:
                    if (!getItem.windAmmunitionFlg) continue;
                    break;
                case bulletType.e_magnet:
                    if (!getItem.magnetAmmunitionFlg) continue;
                    break;
                case bulletType.e_fire:
                    if (!getItem.fireAmmunitionFlg) continue;
                    break;
            }
            break;
        }
    }
}
