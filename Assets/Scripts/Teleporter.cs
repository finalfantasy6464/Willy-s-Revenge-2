using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour{

	public Transform target;

    public AudioClip teleport;

    // Update is called once per frame
   
void OnTriggerEnter2D(Collider2D tele){

		var hit = tele.gameObject;

		if (hit.tag == "Player"){
			hit.transform.position = target.position;
            GameSoundManagement.instance.PlaySingle(teleport);
	}


		if (hit.tag == "Enemy" | hit.tag == "Enemy5"){
			hit.transform.position = target.position;
            GameSoundManagement.instance.PlaySingle(teleport);

        }
			
	}
}