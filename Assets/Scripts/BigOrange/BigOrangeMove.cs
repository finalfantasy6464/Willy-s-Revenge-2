using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BigOrangeMove : ScriptableObject
{
    [HideInInspector] public bool isDone;
    [HideInInspector] public bool executing;
    public event Action OnMoveStart;
    public event Action OnMoveEnd;

    public virtual void Execute()
    {
        OnMoveStart?.Invoke();
        executing = true;
    }

    public virtual void EndCheck()
    {
        if(!isDone) return;
        
        executing = false;
        OnMoveEnd.Invoke();
    }
}
