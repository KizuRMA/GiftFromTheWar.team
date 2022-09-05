using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject explosionHit;
    private List<GameObject> explosionEffectList = new List<GameObject>();      //爆発エフェクトの配列
    private List<GameObject> explosionHitList = new List<GameObject>();         //爆発当たり判定の配列

    void Start()
    {
        explosionEffectList.Add((GameObject)Instantiate(explosionEffect, transform));
        explosionHitList.Add((GameObject)Instantiate(explosionHit, transform));
    }

    void Update()
    {
        //爆発エフェクト終了処理
        if (explosionEffectList.Count != 0)
        {
            if (explosionEffectList[0].transform.childCount <= 0)
            {
                Destroy(explosionEffectList[0]);
                explosionEffectList.RemoveAt(0);
                Destroy(explosionHitList[0]);
                explosionHitList.RemoveAt(0);
            }
        }

        if(explosionEffectList.Count == 0)
        {
            Destroy(gameObject);
        }
    }
}
