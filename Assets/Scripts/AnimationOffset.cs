using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOffset : MonoBehaviour, IPausable
{

	public float NewOffset;
	public float NewSpeed = 1.0f;
    public int AnimIndex = 0;

    public bool isPaused { get; set; }

    public void OnPause()
    {
        GetComponent<Animator>().SetFloat("Speed", 0);
    }

    public void OnUnpause()
    {
        GetComponent<Animator>().SetFloat("Speed", NewSpeed);
    }

    public void PausedUpdate()
    { }

    public void UnPausedUpdate()
    { }

    // Start is called before the first frame update
    void Start()
    {
		GetComponent<Animator>().SetFloat("Offset", NewOffset);
		GetComponent<Animator> ().SetFloat ("Speed", NewSpeed);
        GetComponent<Animator>().SetInteger("AnimIndex", AnimIndex);
    }

}
