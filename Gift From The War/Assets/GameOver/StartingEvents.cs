using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class StartingEvents : MonoBehaviour
{

    [SerializeField] private UnityEvent Events = new UnityEvent();
    private bool onceFlg = false;

    //���X�N���v�g��Start()����ɓ����������̂�Update()�ɏ����Ă���

    void Update()
    {
        if (onceFlg) return;

        Events.Invoke();
        onceFlg = true;
    }

}
