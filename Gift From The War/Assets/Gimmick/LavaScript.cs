using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScript : MonoBehaviour
{
    public bool playerDebug;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Dog1")                                 //犬
        {
            var target = other.transform.GetComponent<EnemyInterface>();
            target.LavaDamage();
            //音を鳴らす
            AudioManager.Instance.PlaySE("BurnLava", gameObject, isLoop: false);
        }
        else if (other.gameObject.tag == "Player" && playerDebug == false)  //プレイヤー
        {
            var target = other.transform.GetComponent<playerAbnormalcondition>();
            if (null == target) return;
            target.Damage(1.0f);
            //音を鳴らす
            AudioManager.Instance.PlaySE("BurnLava", gameObject, isLoop: false);
        }
        else if (other.gameObject.tag == "gimmickButton")                   //ボタン
        {   

            Destroy(other.transform.gameObject);
            //音を鳴らす
            AudioManager.Instance.PlaySE("BurnLava", gameObject, isLoop: false);
        }


    }
}
