using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum e_DogState
{
    Search,
    Traking,
}

public class DogState : StatefulObjectBase<DogState, e_DogState>
{
    void Start()
    {
        //stateList.Add(new StateWander(this));
        //stateList.Add(new StatePursuit(this));

        stateMachine = new StateMachine<DogState>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }
}
