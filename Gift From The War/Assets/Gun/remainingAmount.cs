using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remainingAmount : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    [SerializeField] private GameObject amount;
    [SerializeField] private GameObject amount2;
    [SerializeField] private Transform trans;
    [SerializeField] private MoveWindGun windGun;
    [SerializeField] private Material mat1;
    [SerializeField] private Material mat2;

    //エネルギー残量の表示
    private Vector3 firstPos;                   //基準の位置   
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
        firstPos = transform.position;
        allRemainingEnergy = energyMax - energyMin;
        energyMaxFlg = false;
    }

    void Update()
    {
        if (useEnergy == 0) //エネルギー消費がなかったら
        {
            //エネルギーの回復
            if (recoveryFlg)
            {
                trans.localPosition += new Vector3(0, 0, -(upSpeed * allRemainingEnergy - allRemainingEnergy) * Time.deltaTime);
            }
        }
        else
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

            //エネルギーの色変更
            Material[] tmp = gameObject.GetComponent<Renderer>().materials;
            tmp[0] = mat1;
            amount2.GetComponent<Renderer>().materials = tmp;
        }

        if (trans.localPosition.z > energyMax)
        {
            //エネルギーの最大値
            trans.localPosition = new Vector3(0, 0, energyMax);

            energyMaxFlg = true;

            //エネルギーの色変更
            Material[] tmp = gameObject.GetComponent<Renderer>().materials;
            tmp[0] = mat2;
            amount2.GetComponent<Renderer>().materials = tmp;
        }
        else if (trans.localPosition.z < energyMin)
        {
            //エネルギーの最小値
            trans.localPosition = new Vector3(0, 0, energyMin);
        }

        nowRemainingEnergy = 1.0f - (energyMax - trans.localPosition.z) / allRemainingEnergy;
    }

    private IEnumerator RecoveryCoolTime()  //回復までのクールタイム
    {
        yield return new WaitForSeconds(coolTime);  //クールタイム分待つ

        recoveryFlg = true;
    }

    public float GetSetNowAmount
    {
        get { return nowRemainingEnergy; }
        set { useEnergy = value; }
    }
}
