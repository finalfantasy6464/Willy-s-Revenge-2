using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BigOrangeMove
{
    public bool isDone;
    public event Action OnMoveStart;
    public event Action OnMoveEnd;

    public void Execute()
    {
        OnMoveStart?.Invoke();
        
    }

    public virtual bool MoveUpdate()
    {
        if(isDone)
            OnMoveEnd.Invoke();
        return isDone;
    }
}
