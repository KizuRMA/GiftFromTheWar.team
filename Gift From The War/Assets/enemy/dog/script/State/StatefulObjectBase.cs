using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatefulObjectBase<T, TEnum> : MonoBehaviour
    where T : class where TEnum : System.IConvertible
{
    protected List<State<T>> stateList = new List<State<T>>();

    protected StateMachine<T> stateMachine;

    public virtual void ChangeState(TEnum state)
    {
        if (stateMachine == null || stateList.Count <= 0)
        {
            return;
        }

        stateMachine.ChangeState(stateList[state.ToInt32(null)]);
    }

    public virtual bool IsCurrentState(TEnum state)
    {
        if (stateMachine == null)
        {
            return false;
        }

        return stateMachine.CurrentState == stateList[state.ToInt32(null)];
    }

    protected virtual void Update()
    {
        if (stateMachine != null)
        {
            stateMachine.Update();
        }
    }
}