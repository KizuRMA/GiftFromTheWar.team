using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{

    public int CurrentState { set; get; }
    protected BatController myController;
    protected BaseUltrasound ultrasound = null;

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public void ChageUltrasound(BaseUltrasound _base)
    {
        //実体を削除
        ultrasound = null;
        //新しい実体のアドレスを入れる
        ultrasound = _base;
        ultrasound.Start();
    }
}
