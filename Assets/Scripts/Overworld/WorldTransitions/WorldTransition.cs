using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldTransition : MonoBehaviour
{
    public Behaviour[] toDisable;

    bool secondTraversed;
    IEnumerator forwardRoutine;
    IEnumerator backwardRoutine;
    WorldTransitionNode first;
    WorldTransitionNode second;

    void Start()
    {
        first = transform.GetChild(0).GetComponent<WorldTransitionNode>();
        second = transform.GetChild(1).GetComponent<WorldTransitionNode>();
    }

    public void OnEnterNode(WorldTransitionNode node)
    {
        if(node == first && !secondTraversed)
            OnEnterForward();
        else if(node == second && !secondTraversed)
            OnEnterBackward();
        else
            Debug.LogError("Unrecognized node in World Transition");
    }

    void OnEnterForward()
    {
        if(forwardRoutine != null)
        {
            StopCoroutine(forwardRoutine);
            forwardRoutine = null;
            Debug.LogWarning("ForwardRoutine hadn't finished and had to be stopped");
        }
        OnTransitionStart();
        StartCoroutine(ForwardRoutine());
    }

    void OnEnterBackward()
    {
        if(backwardRoutine != null)
        {
            StopCoroutine(backwardRoutine);
            backwardRoutine = null;
            Debug.LogWarning("BackwardRoutine hadn't finished and had to be stopped");
        }
        OnTransitionStart();
        StartCoroutine(ForwardRoutine());
    }

    void OnTransitionStart()
    {
        secondTraversed = false;
        foreach (Behaviour b in toDisable)
        {
            b.enabled = false;
            Debug.Log("Disabling behaviour");
            //Or other stopping code here, implement Interface if necessary.
        }
    }

    protected void OnTransitionEnd()
    {
        secondTraversed = true;
        foreach (Behaviour b in toDisable)
            b.enabled = true;
    }

    protected abstract IEnumerator ForwardRoutine();
    protected abstract IEnumerator BackwardRoutine();
}
