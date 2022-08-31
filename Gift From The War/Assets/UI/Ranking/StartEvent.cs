using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class StartEvent : MonoBehaviour
{

    [SerializeField] private UnityEvent StartEvents = new UnityEvent();
    [SerializeField] private UnityEvent NameEvents = new UnityEvent();

    private bool onceFlg = false;
    // Start is called before the first frame update
    void Start()
    {
        StartEvents.Invoke();
    }

    void Update()
    {
        if (!onceFlg && InputNameFlg.nameFlg)
        {
            NameEvents.Invoke();
            onceFlg = true;
        }
    }

}
