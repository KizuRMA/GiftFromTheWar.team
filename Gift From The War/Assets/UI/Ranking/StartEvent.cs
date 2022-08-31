using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class StartEvent : MonoBehaviour
{

    [SerializeField] private UnityEvent StartEvents = new UnityEvent();


    // Start is called before the first frame update
    void Start()
    {
        StartEvents.Invoke();
    }

}
