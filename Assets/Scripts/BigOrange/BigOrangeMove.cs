using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BigOrangeMove : ScriptableObject
{
    public bool isDone;
    public bool executing;
    public event Action OnMoveStart;
    public event Action OnMoveEnd;

    public virtual void Execute()
    {
        OnMoveStart?.Invoke();
        executing = true;
    }

    void Update()
    {
        if(executing)
            MoveUpdate();
    }

    public virtual void MoveUpdate()
    {
        if(!isDone) return;
        
        isDone = true;
        executing = false;
        OnMoveEnd.Invoke();
    }
}
