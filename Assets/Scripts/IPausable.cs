using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPausable
{
    bool isPaused { get; set;}
    void OnPause();
    void OnUnpause();
    void PausedUpdate();
    void UnPausedUpdate();
}
