using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindGun : MonoBehaviour
{
    private ParticleSystem par;
    [SerializeField] private MoveWindGun wind;

    private bool effectFlg = false;

    void Start()
    {
        par = this.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!wind.effectFlg)    //風の力を使っていなかったら非表示
        {
            effectFlg = false;
            par.Stop();
            return;    
        }

        if (effectFlg) return;  //すでにエフェクトを出していたら処理しない

        effectFlg = true;
        par.Play();
    }
}
