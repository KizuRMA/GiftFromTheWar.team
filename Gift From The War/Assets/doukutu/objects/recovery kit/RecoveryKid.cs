using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryKid : MonoBehaviour
{
    [SerializeField] private GameObject recoveryEffect;
    [SerializeField] private playerAbnormalcondition HP;
    private List<GameObject> recoveryEffectList = new List<GameObject>();
    public Vector3 effectPos;
    public bool effectOnFlg = false;

    void Start()
    {
        effectOnFlg = false;
        recoveryEffectList = new List<GameObject>();
    }

    void Update()
    {
        if(effectOnFlg)
        {
            effectOnFlg = false;
            Transform trans = recoveryEffect.transform;
            trans.position = effectPos;
            recoveryEffectList.Add((GameObject)Instantiate(recoveryEffect, trans));

            HP.life = 3;

            AudioManager.Instance.PlaySE("ÉQÅ[ÉWâÒïú3");
        }

        if (recoveryEffectList.Count != 0)
        {
            if (recoveryEffectList[0].transform.childCount <= 0)
            {
                Destroy(recoveryEffectList[0]);
                recoveryEffectList.RemoveAt(0);
            }
        }
    }
}
