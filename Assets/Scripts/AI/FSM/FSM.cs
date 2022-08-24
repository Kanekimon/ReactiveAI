using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    private Stack<IFSMState> _states = new Stack<IFSMState>();

    public delegate void IFSMState(FSM fsm, GameObject gameObject);


    public void Update(GameObject gameObject)
    {
        if (_states.Peek() != null)
        {
            _states.Peek().Invoke(this, gameObject);
        }
    }

    public void PushState(IFSMState state)
    {
        _states.Push(state);
    }

    public void PopState()
    {
        _states.Pop();
    }

}

