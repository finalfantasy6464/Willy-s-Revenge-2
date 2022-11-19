using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatePressurePlate : MonoBehaviour
{
    public event Action OnStepIn;
    public event Action OnStepOut;

    public void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out OverworldPlayer player))
            OnStepIn.Invoke();
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out OverworldPlayer player))
            OnStepOut.Invoke();
    }
}
