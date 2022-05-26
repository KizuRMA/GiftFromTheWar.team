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
    private float nowRemainingAmount;           //今の残量
    private float useAmount;                    //消費量
    private float allRemainingAmount;           //計算に必要な定数
    [SerializeField] private float upSpeed;     //回復スピード
    [SerializeField] private float amountMax;   //最大量
    [SerializeField] private float amountMin;   //最小量

    void Start()
    {
        firstPos = transform.position;
        allRemainingAmount = amountMax - amountMin;
    }

    void Update()
    {
        if (useAmount == 0) //エネルギー消費がなかったら
        {
            //エネルギーの回復
            trans.localPosition += new Vector3(0, 0, -(upSpeed * allRemainingAmount - allRemainingAmount) * Time.deltaTime);
        }
        else
        {
            //エネルギーの消費処理
            trans.localPosition += new Vector3(0, 0, (useAmount * allRemainingAmount - allRemainingAmount) * Time.deltaTime);

            //エネルギーの色変更
            Material[] tmp = gameObject.GetComponent<Renderer>().materials;
            tmp[0] = mat1;
            amount2.GetComponent<Renderer>().materials = tmp;
        }

        if (trans.localPosition.z > amountMax)
        {
            //エネルギーの最大値
            trans.localPosition = new Vector3(0, 0, amountMax);

            //エネルギーの色変更
            Material[] tmp = gameObject.GetComponent<Renderer>().materials;
            tmp[0] = mat2;
            amount2.GetComponent<Renderer>().materials = tmp;
        }
        else if (trans.localPosition.z < amountMin)
        {
            //エネルギーの最小値
            trans.localPosition = new Vector3(0, 0, amountMin);
        }

        nowRemainingAmount = 1.0f - (amountMax - trans.localPosition.z) / allRemainingAmount;
    }

    public float GetSetNowAmount
    {
        get { return nowRemainingAmount; }
        set { useAmount = value; }
    }
}
