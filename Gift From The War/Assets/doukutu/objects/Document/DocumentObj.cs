using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentObj : MonoBehaviour
{
    [SerializeField] public DocumentOpen document;
    [SerializeField] public BoxCollider collider;
    float coolTime;

    private void Start()
    {
        coolTime = 0;
        collider.enabled = true;
    }

    public void DocumentOpen()
    {
        if (document == null || coolTime > 0) return;
        document.Open();
        coolTime = 1;
        collider.enabled = false;
    }

    private void Update()
    {
        if (SystemSetting.Instance.pauseType == SystemSetting.e_PauseType.Document || coolTime <= 0) return;
        coolTime -= Time.deltaTime;
        if (coolTime <= 0)
        {
            coolTime = 0;
            collider.enabled = true;
        }
    }

}
