using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particlesystemtoggler : MonoBehaviour, IPausable
{
    ParticleSystem particle;

    public bool isPaused { get; set; }

    public void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }
    public void OnPause()
    {
        if (particle.isPlaying)
        {
            particle.Pause();
        }
    }

    public void OnUnpause()
    {
        if (particle.isPaused)
        {
            particle.Play();
        }
    }

    public void PausedUpdate()
    { }

    public void UnPausedUpdate()
    { }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}
