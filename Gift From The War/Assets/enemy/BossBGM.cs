using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBGM : MonoBehaviour
{
    [SerializeField] private float waitTime;
    [SerializeField] private float vol;
    [SerializeField] private List<GameObject> enemysInfo = null;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public IEnumerator BGMPlay()
    {
        yield return new WaitForSeconds(waitTime);

        foreach(var i in enemysInfo)
        {
            i.SetActive(false);
        }

        AudioManager.Instance.PlaySE("Heartbeat");
        AudioManager.Instance.PlayBGM("boss", vol: vol);
    }
}
