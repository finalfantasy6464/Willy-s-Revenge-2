using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AnimationEnabler : MonoBehaviour, IPausable
{
	Animator m_Animator;

    public AudioClip clip;

    public bool isPaused { get; set; }

    void Start()
	{
		//Get the Animator attached to the GameObject you are intending to animate.
		m_Animator = gameObject.GetComponent<Animator>();
    }

	void OnTriggerEnter2D(Collider2D coll)
	{
		var hit = coll.gameObject;

		if (hit.tag == "Jump")
		{
            m_Animator.Play("PlayerJump");
            GameSoundManagement.instance.PlayOneShot(clip);
		}
	}

    public void OnPause()
    {
        m_Animator.SetFloat("Speed", 0);
    }

    public void OnUnpause()
    {
        m_Animator.SetFloat("Speed", 1);
    }

    public void PausedUpdate()
    {
    }

    public void UnPausedUpdate()
    {
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}
