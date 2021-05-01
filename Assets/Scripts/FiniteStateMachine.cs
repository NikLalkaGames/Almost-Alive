using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FiniteStateMachine
{
    private Action stateOfBehaviour;
    public void SetUpState(Action stateToSetUp)
    {
        stateOfBehaviour = stateToSetUp;
    }
    public void TransitTo(Action stateToChange)
    {
        stateOfBehaviour = stateToChange;
    }
    public void UpdateState()
    {
        stateOfBehaviour();
    }
}