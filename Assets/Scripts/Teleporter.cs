using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour{

	public Transform target;

    public PositionalSFX sfx;
    public PositionalSFX sfx2;

    public ParticleSystem teleParticleIn;
    public ParticleSystem teleParticleOut;

    // Update is called once per frame
   
void OnTriggerEnter2D(Collider2D tele){

		var hit = tele.gameObject;

		if (hit.tag == "Player"){
			hit.transform.position = target.position;
            sfx.PlayPositionalSound();
            sfx2.PlayPositionalSound();
            teleParticleIn.Play();
            teleParticleOut.Play();
    }


		if (hit.tag == "Enemy" | hit.tag == "Enemy5"){
			hit.transform.position = target.position;
            sfx.PlayPositionalSound();
            sfx2.PlayPositionalSound();
            teleParticleIn.Play();
            teleParticleOut.Play();
        }
			
	}
}