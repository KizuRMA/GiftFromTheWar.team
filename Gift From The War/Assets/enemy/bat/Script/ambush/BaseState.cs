using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{

    public int CurrentState { set; get; }
    protected BatController myController;
    protected BaseUltrasound ultrasound = null;

    public virtual void Init()
    {

    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {
        if (ultrasound != null)
        {
            ultrasound.Init();
            ultrasound.Exit();
        }
    }

    public void ChangeUltrasound(BaseUltrasound _base)
    {
        //���̂��폜
        if (ultrasound != null)
        {
            ultrasound.Init();
            ultrasound.Exit();
        }
        ultrasound = null;
        //�V�������̂̃A�h���X������
        ultrasound = _base;
        ultrasound.Start();
    }
}
