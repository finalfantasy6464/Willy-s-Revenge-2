using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOffset : MonoBehaviour, IPausable
{
    public float corruptionDeathTime;
	public float NewOffset;
	public float NewSpeed = 1.0f;
    public int AnimIndex = 0;

    public bool isPaused { get; set; }

    Animator myAnimator;
    SpriteRenderer myRenderer;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
		myAnimator.SetFloat("Offset", NewOffset);
		myAnimator.SetFloat ("Speed", NewSpeed);
        if(AnimIndex != -1)
            myAnimator.SetInteger("AnimIndex", AnimIndex);
    }

    public void OnPause()
    {
        myAnimator.SetFloat("Speed", 0);
    }

    public void OnUnpause()
    {
        myAnimator.SetFloat("Speed", NewSpeed);
    }

    public void UnPausedUpdate() { }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}
