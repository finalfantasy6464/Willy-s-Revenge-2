using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGate : MonoBehaviour
{
	public SwitchCollide3[] switchcollide3;

	public Sprite changesprite;

	public int switchno;

	public bool allpressed;

	public AudioClip Allpressed;

	void Update ()
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Switch");

		for (int i = 0; i < gos.Length; i++) {
			switchcollide3 [i] = gos [i].GetComponent<SwitchCollide3> ();

			if (switchcollide3 [i].Active & switchno == i + 1) {
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = changesprite;
			}
				

			bool allpressed = true;
			foreach (SwitchCollide3 sw in switchcollide3) {
				if (sw.Active == false) {
					allpressed = false;
					break;
				}
			}
				if (allpressed){
				if(Allpressed != null)
                {
					GameSoundManagement.instance.PlayOneShot(Allpressed);
				}
					Destroy (gameObject);
	}
}
}
}