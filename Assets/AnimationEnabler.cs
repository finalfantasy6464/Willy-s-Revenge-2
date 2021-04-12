using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnabler : MonoBehaviour
{
	Animator m_Animator;
	Collider2D m_Collider;

	private float resettimer;

	private bool justjumped = false;

	void Start()
	{
		//Get the Animator attached to the GameObject you are intending to animate.
		m_Animator = gameObject.GetComponent<Animator>();
		m_Collider = gameObject.GetComponent<Collider2D> ();
	}

	void Update(){
		resettimer += Time.deltaTime;

		if (resettimer >= 3.5f & justjumped == true) {
			m_Animator.SetTrigger ("Jump");
			resettimer = 0.0f;
			justjumped = false;
			m_Collider.enabled = true;
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		var hit = coll.gameObject;

		if (hit.tag == "Jump")
		{
			m_Animator.SetTrigger("Jump");
			justjumped = true;
			resettimer = 0.0f;
			m_Collider.enabled = false;
		}
	}
}
