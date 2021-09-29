using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPausable
{
    bool isPaused { get; set;}
    void OnPause();
    void OnUnpause();
    void OnDestroy();
    void PausedUpdate();
    void UnPausedUpdate();
}
