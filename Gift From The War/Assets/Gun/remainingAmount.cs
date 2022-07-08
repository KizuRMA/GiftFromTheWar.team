using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remainingAmount : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    [SerializeField] private GameObject amount;
    [SerializeField] private GameObject amount2;
    [SerializeField] private Transform trans;
    [SerializeField] private Material matWind;
    [SerializeField] private Material matWindMax;
    [SerializeField] private Material matMagnet;
    [SerializeField] private Material matMagnetMax;
    [SerializeField] private Material matFire;
    [SerializeField] private Material matFireMax;
    [SerializeField] private bulletChange bulletChange;

    //エネルギー残量の表示
    private float nowRemainingEnergy;           //今の残量
    private float useEnergy;                    //消費量
    private float allRemainingEnergy;           //計算に必要な定数
    [SerializeField] private float upSpeed;     //回復スピード
    [SerializeField] private float energyMax;   //最大量
    [SerializeField] private float energyMin;   //最小量
    public bool useDeltaTime { get; set; }     //デルタタイムを使うか

    //エネルギー回復のクールタイム
    [SerializeField] private int coolTime;      //エネルギー回復のクールタイム
    private bool recoveryFlg = false;           //回復するフラグ
    private IEnumerator cor;                    //持っているコルーチン

    public bool energyMaxFlg { get; set; } //エネルギーが最大がどうか

    void Start()
    {
        allRemainingEnergy = energyMax - energyMin;
        energyMaxFlg = false;
    }

    void Update()
    {
        if (useEnergy == 0) //エネルギー消費がなかったら
        {
            NoUseEnergy();
        }
        else
        {
            UseEnergy();
        }

        EnergyMax();

        ColorChange();

        nowRemainingEnergy = 1.0f - (energyMax - trans.localPosition.z) / allRemainingEnergy;
    }

    private IEnumerator RecoveryCoolTime()  //回復までのクールタイム
    {
        yield return new WaitForSeconds(coolTime);  //クールタイム分待つ

        recoveryFlg = true;
    }

    private void NoUseEnergy()  //エネルギー消費がなかったら
    {
        //エネルギーの回復
        if (recoveryFlg)
        {
            trans.localPosition += new Vector3(0, 0, -(upSpeed * allRemainingEnergy - allRemainingEnergy) * Time.deltaTime);
        }
    }

    private void UseEnergy()    //エネルギー消費があったら
    {
        recoveryFlg = false;
        energyMaxFlg = false;

        //一つのコルーチンを使いまわす
        if (cor != null) StopCoroutine(cor);
        cor = null;
        cor = RecoveryCoolTime();
        StartCoroutine(cor);

        //エネルギーの消費処理
        if (useDeltaTime)
        {
            trans.localPosition += new Vector3(0, 0, (useEnergy * allRemainingEnergy - allRemainingEnergy) * Time.deltaTime);
        }
        else
        {
            trans.localPosition += new Vector3(0, 0, (useEnergy * allRemainingEnergy - allRemainingEnergy));
        }
    }

    private void EnergyMax()    //エネルギーが最大までたまっていたら
    {
        if (trans.localPosition.z > energyMax)
        {
            //エネルギーの最大値
            trans.localPosition = new Vector3(0, 0, energyMax);

            energyMaxFlg = true;
        }
        else if (trans.localPosition.z < energyMin)
        {
            //エネルギーの最小値
            trans.localPosition = new Vector3(0, 0, energyMin);
        }
    }

    private void ColorChange()  //エネルギーの色変更
    {
        if (bulletChange.cylinder.isChanging == true) return;

        Material[] tmp = gameObject.GetComponent<Renderer>().materials;

        if (trans.localPosition.z < energyMax)
        {
            //弾の種類による、色の変化
            switch (bulletChange.nowBulletType)
            {
                case bulletChange.bulletType.e_wind:
                    tmp[0] = matWind;
                    break;

                case bulletChange.bulletType.e_magnet:
                    tmp[0] = matMagnet;
                    break;

                case bulletChange.bulletType.e_fire:
                    tmp[0] = matFire;
                    break;
            }
        }
        else
        {
            //弾の種類による、色の変化
            switch (bulletChange.nowBulletType)
            {
                case bulletChange.bulletType.e_wind:
                    tmp[0] = matWindMax;
                    break;

                case bulletChange.bulletType.e_magnet:
                    tmp[0] = matMagnetMax;
                    break;

                case bulletChange.bulletType.e_fire:
                    tmp[0] = matFireMax;
                    break;
            }
        }

        amount2.GetComponent<Renderer>().materials = tmp;
    }

    public float GetSetNowAmount
    {
        get { return nowRemainingEnergy; }
        set { useEnergy = value; }
    }
}
