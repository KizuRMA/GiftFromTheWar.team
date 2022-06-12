using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBulletHit : MonoBehaviour
{
    private Transform trans;

    [SerializeField] private float scaleSpeed;
    [SerializeField] private float scaleFirst;
    [SerializeField] private float scaleMax;
    private float nowScale;

    void Start()
    {
        trans = transform;
        nowScale = scaleFirst;
    }

    void Update()
    {
        nowScale += scaleSpeed * Time.deltaTime;

        nowScale = nowScale > scaleMax ? scaleMax : nowScale;

        trans.localScale = new Vector3(nowScale, nowScale, nowScale);
    }
}