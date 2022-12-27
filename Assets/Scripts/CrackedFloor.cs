using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class CrackedFloor : MonoBehaviour
{
	public Sprite Sprite1;
	public Sprite Sprite2;
	bool touchedonce;

    AudioSource source;
    public AudioClip clip;

    float minpitch = 0.95f;
    float maxpitch = 1.05f;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    void OnCollisionEnter2D(Collision2D coll){

		if (touchedonce == true)
		{
		var sprite = coll.gameObject;

			if (sprite.tag == "Player") 
			{
				Destroy (sprite);
				SceneManager.LoadScene (SceneManager.GetActiveScene().name);
			}
		}
        else
        {
				this.GetComponent<SpriteRenderer>().sprite = Sprite1;
		}
	}

	void OnCollisionExit2D(Collision2D coll) {

		var sprite = coll.gameObject;

		if (sprite.tag == "Player") {
			this.GetComponent<SpriteRenderer> ().sprite = Sprite2;
			touchedonce = true;
            source.pitch = Random.Range(minpitch, maxpitch);
            source.PlayOneShot(clip);
		}
	}
}
