using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionInteraction : MonoBehaviour
{
	ParticleSystem particles;
	ParticleSystem.EmissionModule emissionModule;
	float defaultEmissionrate = 0f;
	public float newEmissionrate;

    private void Start()
    {
		particles = GetComponent<ParticleSystem>();
		emissionModule = particles.emission;
		emissionModule.rateOverTime = defaultEmissionrate;
    }
    void OnTriggerEnter2D(Collider2D coll)
	{
		var hit = coll.gameObject;

		if (hit.CompareTag("Corruption"))
		{
			emissionModule.rateOverTime = newEmissionrate;
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		var hit = coll.gameObject;
		if (hit.CompareTag("Corruption"))
		{
			emissionModule.rateOverTime = defaultEmissionrate;
		}
	}
}
