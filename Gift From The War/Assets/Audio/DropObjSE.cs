using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjSE : MonoBehaviour
{
    [SerializeField] private string SEName;
    [SerializeField] private float minVol;  //ボリュームの最小値
    [SerializeField] private float velocityVolRaito;  //高さによるボリュームの補正倍率
    [SerializeField] private float volDis;

    private float pastPosY; //オブジェクトの前の座標保存しておく
    private bool moveFlg;   //オブジェクトが動いたか

    void Start()
    {
    }

    void Update()
    {
        moveFlg = (pastPosY != this.transform.position.y);
        pastPosY = this.transform.position.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!moveFlg) return;
        if (collision.transform.tag == "Detector") return;

        moveFlg = false;
        float minHeight = this.transform.position.y;

        //補正計算
        float velocityVol = this.GetComponent<Rigidbody>().velocity.magnitude * velocityVolRaito;

        AudioManager.Instance.PlaySE(SEName, this.gameObject, volDis, isLoop: false, vol: minVol + velocityVol);
    }
}
