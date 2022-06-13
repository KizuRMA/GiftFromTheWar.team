using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compass : MonoBehaviour
{
    [SerializeField] private GameObject goalPos;
    private Transform trans;
    private Transform goaltrans;

    void Start()
    {
        trans = transform;
        goaltrans = goalPos.transform;
    }

    void Update()
    {
        Vector3 compassVec = goaltrans.position;
        compassVec.y = trans.position.y;
        trans.LookAt(compassVec);
        trans.localEulerAngles += new Vector3(-90, 180, 0);
    }
}
