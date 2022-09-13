using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScript : MonoBehaviour
{
    public bool playerDebug;

    private void OnTriggerStay(Collider other)
    {
        //��
        if (other.gameObject.tag == "Dog1")
        {
            var target = other.transform.GetComponent<EnemyInterface>();
            target.LavaDamage();
        }

        //�v���C���[
        if (other.gameObject.tag == "Player" && playerDebug == false)
        {
            var target = other.transform.GetComponent<playerAbnormalcondition>();
            if (null == target) return;
            target.Damage(1.0f);
        }

        //�{�^��
        if (other.gameObject.tag == "gimmickButton")
        {
            Destroy(other.transform.gameObject);
        }

        //����炷
        AudioManager.Instance.PlaySE("BurnLava", gameObject, isLoop: false);
    }
}
