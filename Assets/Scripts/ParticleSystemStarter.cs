using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemStarter : MonoBehaviour
{
    ParticleSystem particle;
    ParticleSystem.EmissionModule emissionModule;
    float emissionRate = 0f;
    public float newemissionrate = 20f;
    float particlestep;
    float particletimer = 0.2f;

    bool startcomplete = false;

    public void Start()
    {
        particle = GetComponent<ParticleSystem>();
        emissionModule = particle.emission;
        emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(emissionRate);
    }

    private void Update()
    {
        if (!startcomplete)
        {
            particlestep += Time.deltaTime;

            if (particlestep > particletimer)
            {
                emissionRate = newemissionrate;
                emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(emissionRate);
                startcomplete = true;
            }
        }
    }
}
