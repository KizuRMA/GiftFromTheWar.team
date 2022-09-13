using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScript : MonoBehaviour
{
    public bool playerDebug;

    private void OnTriggerStay(Collider other)
    {
        //犬
        if (other.gameObject.tag == "Dog1")
        {
            var target = other.transform.GetComponent<EnemyInterface>();
            target.LavaDamage();
        }

        //プレイヤー
        if (other.gameObject.tag == "Player" && playerDebug == false)
        {
            var target = other.transform.GetComponent<playerAbnormalcondition>();
            if (null == target) return;
            target.Damage(1.0f);
        }

        //ボタン
        if (other.gameObject.tag == "gimmickButton")
        {
            Destroy(other.transform.gameObject);
        }

        //音を鳴らす
        AudioManager.Instance.PlaySE("BurnLava", gameObject, isLoop: false);
    }
}
