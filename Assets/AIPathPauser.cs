using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIPathPauser : MonoBehaviour, IPausable
{
    AIBase aicontrol;

    public bool isPaused { get; set; }

    public void Start()
    {
        aicontrol = GetComponent<AIBase>();
    }

    public void OnPause()
    {
        aicontrol.canMove = false;
    }
    public void OnUnpause()
    {
        aicontrol.canMove = true;
    }


    public void UnPausedUpdate()
    { }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}
