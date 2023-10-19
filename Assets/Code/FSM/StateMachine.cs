using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    
    #region Protected Fields

    protected State State;

    #endregion

    #region Public Methods

    public void SetState(State state)
    {
        State?.EndState();
        State = state;
        StartCoroutine(routine: State.Start());
    }

    public State GetState() { return State; }

    #endregion
    
}
