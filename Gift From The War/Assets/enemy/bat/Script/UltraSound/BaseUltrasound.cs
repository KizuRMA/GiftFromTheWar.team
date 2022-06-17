using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUltrasound : MonoBehaviour
{
    protected GameObject playerObject;
    public float coolDown { set; get; }
    protected float duration;
    protected float time;
    protected float range;
    protected float maxRange;
    protected float velocity;
    protected bool aliveFlg;

    public bool IsAlive => aliveFlg == true;

    private void Awake()
    {
        playerObject = GameObject.Find("player").gameObject;
    }

    public virtual void Init()
    {

    }

    public virtual void Start()
    {
        coolDown = 0;
    }

    public virtual void Update()
    {

    }

    public virtual bool CheckHit()
    {
        return false;
    }

    public virtual void DrawLine()
    {

    }

    public virtual void Exit()
    {

    }
}
