using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject explosionHit;
    private List<GameObject> explosionEffectList = new List<GameObject>();      //�����G�t�F�N�g�̔z��
    private List<GameObject> explosionHitList = new List<GameObject>();         //���������蔻��̔z��

    void Start()
    {
        
    }

    void Update()
    {
        //�����G�t�F�N�g�I������
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

        if(Input.GetKey(KeyCode.E))
        {
            if (explosionEffectList.Count >= 1) return;
            explosionEffectList.Add((GameObject)Instantiate(explosionEffect, new Vector3 (-73.49f, -1.59f, 231.56f), Quaternion.identity));
            explosionHitList.Add((GameObject)Instantiate(explosionHit, new Vector3(-73.49f, -1.59f, 231.56f), Quaternion.identity));
        }

        Debug.Log(explosionEffectList.Count);
    }
}